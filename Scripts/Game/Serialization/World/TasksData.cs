using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class TasksData<TaskData, Info> : IMonthUpdatable
        where TaskData : TaskData<Info>
        where Info : TaskInfo
    {
        #region fields & properties
        public UnityAction<TaskData> OnTaskStarted;
        public UnityAction<TaskData> OnTaskCompleted;
        public UnityAction<TaskData> OnTaskExpired;
        public IReadOnlyList<TaskData> StartedTasks => startedTasks;
        [SerializeField] private List<TaskData> startedTasks = new();
        public IReadOnlyList<TaskData> CompletedTasks => completedTasks;
        [SerializeField] private List<TaskData> completedTasks = new();
        public IReadOnlyList<TaskData> ExpiredTasks => expiredTasks;
        [SerializeField] private List<TaskData> expiredTasks = new();
        #endregion fields & properties

        #region methods
        public virtual void OnMonthUpdate(MonthData monthData)
        {
            CheckTasksExpiration();
        }
        private void CheckTasksExpiration()
        {
            int itemsCount = startedTasks.Count;
            for (int i = itemsCount - 1; i >= 0; --i)
            {
                TaskData startedTask = startedTasks[i];
                if (!startedTask.IsExpired()) continue;
                SetTaskExpired(startedTask);
            }
        }
        public bool TrySetTaskExpired(int startedTaskId)
        {
            if (!IsIdCorrect(startedTaskId)) return false;
            if (!IsTaskStarted(startedTaskId, out TaskData started)) return false;
            SetTaskExpired(started);
            return true;
        }
        private void SetTaskExpired(TaskData startedTask)
        {
            startedTasks.Remove(startedTask);
            int currentId = startedTask.Id;
            expiredTasks.Add(startedTask);
            OnBeforeTaskExpired(startedTask);
            OnTaskExpired?.Invoke(startedTask);
        }
        /// <summary>
        /// Invokes just before the action
        /// </summary>
        protected virtual void OnBeforeTaskExpired(TaskData expired) { }
        private bool IsTaskExistInList(IReadOnlyList<TaskData> list, int id, out TaskData exist)
        {
            if (id < 0)
            {
                exist = null;
                return false;
            }
            int itemsCount = list.Count;
            for (int i = 0; i < itemsCount; ++i)
            {
                if (list[i].Id != id) continue;
                exist = list[i];
                return true;
            }
            exist = null;
            return false;
        }
        protected void RemoveStartedTask(TaskData task) => startedTasks.Remove(task);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if task started/completed/expired</returns>
        protected bool IsTaskExist(int id, out TaskData exist)
        {
            if (IsTaskExistInList(startedTasks, id, out exist)) return true;
            if (IsTaskExistInList(completedTasks, id, out exist)) return true;
            if (IsTaskExistInList(expiredTasks, id, out exist)) return true;
            return false;
        }
        public bool IsTaskStarted(int id, out TaskData started) => IsTaskExistInList(startedTasks, id, out started);
        public bool IsTaskCompleted(int id, out TaskData completed) => IsTaskExistInList(completedTasks, id, out completed);
        public bool IsTaskExpired(int id, out TaskData completed) => IsTaskExistInList(expiredTasks, id, out completed);
        private static bool IsIdCorrect(int id) => id > -1;
        public virtual bool CanStartTask(int id)
        {
            if (IsTaskExist(id, out _)) return false;
            return true;
        }
        public bool TryStartTask(int id)
        {
            if (!IsIdCorrect(id)) return false;
            if (!CanStartTask(id)) return false;
            TaskData newTask = CreateNewTask(id);
            startedTasks.Add(newTask);
            OnTaskStarted?.Invoke(newTask);
            return true;
        }
        public bool TryCompleteTask(int id)
        {
            if (!IsIdCorrect(id)) return false;
            if (!IsTaskStarted(id, out TaskData found)) return false;
            Info info = found.Info;
            AddReward(info);
            startedTasks.Remove(found);
            completedTasks.Add(found);
            OnBeforeTaskCompleted(found);
            OnTaskCompleted?.Invoke(found);
            return true;
        }
        protected virtual void AddReward(Info taskInfo) => taskInfo.RewardInfo.AddReward();
        protected abstract TaskData CreateNewTask(int id);
        /// <summary>
        /// Invokes just before the action
        /// </summary>
        /// <param name="task"></param>
        protected virtual void OnBeforeTaskCompleted(TaskData task) { }
        #endregion methods
    }
}