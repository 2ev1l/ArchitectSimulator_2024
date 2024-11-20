using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public abstract class TasksObserver<TaskData, Info> : Observer where TaskData : TaskData<Info> where Info : TaskInfo
    {
        #region fields & properties
        protected abstract TasksData<TaskData, Info> Context { get; }
        #endregion fields & properties

        #region methods
        public override void Dispose()
        {
            Context.OnTaskStarted -= OnTaskStarted;
            Context.OnTaskCompleted -= OnTaskCompleted;
            Context.OnTaskExpired -= OnTaskExpired;
        }
        public override void Initialize()
        {
            Context.OnTaskStarted += OnTaskStarted;
            Context.OnTaskCompleted += OnTaskCompleted;
            Context.OnTaskExpired += OnTaskExpired;
        }
        protected abstract void OnTaskStarted(TaskData taskData);
        protected abstract void OnTaskCompleted(TaskData taskData);
        protected abstract void OnTaskExpired(TaskData taskData);
        protected bool TryStartTask(int id) => Context.TryStartTask(id);
        protected bool TryCompleteTask(int id) => Context.TryCompleteTask(id);
        #endregion methods
    }
}