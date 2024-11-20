using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Events
{
    //Works only with player tasks
    public class TaskChecker : ResultChecker
    {
        #region fields & properties
        private static PlayerTasksData Context => GameData.Data.PlayerData.Tasks;

        [SerializeField][Min(0)] private int taskId;
        [SerializeField] private TaskCheckState checkState;
        [SerializeField] private ResultCombineOperator combineOperator = ResultCombineOperator.Or;
        [SerializeField] private bool subscribeAtChange = true;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            if (subscribeAtChange)
            {
                GameData.Data.PlayerData.MonthData.OnMonthChanged += Check;
                Context.OnTaskCompleted += Check;
                Context.OnTaskStarted += Check;
            }
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            if (subscribeAtChange)
            {
                GameData.Data.PlayerData.MonthData.OnMonthChanged -= Check;
                Context.OnTaskCompleted -= Check;
                Context.OnTaskStarted -= Check;
            }
            base.OnDisable();
        }
        private void Check(int _) => Check();
        private void Check(PlayerTaskData _)
        {
            Check();
        }

        public override bool GetResult()
        {
            bool result = combineOperator.GetStartResult();
            bool isStarted = Context.IsTaskStarted(taskId, out _);
            bool isCompleted = Context.IsTaskCompleted(taskId, out _);
            bool isExpired = Context.IsTaskExpired(taskId, out _);
            bool exist = isStarted || isCompleted || isExpired;
            if (checkState.HasFlag(TaskCheckState.NotExist))
            {
                result = combineOperator.Execute(result, !exist);
            }
            if (checkState.HasFlag(TaskCheckState.Started))
            {
                result = combineOperator.Execute(result, isStarted);
            }
            if (checkState.HasFlag(TaskCheckState.Completed))
            {
                result = combineOperator.Execute(result, isCompleted);
            }
            if (checkState.HasFlag(TaskCheckState.Expired))
            {
                result = combineOperator.Execute(result, isExpired);
            }
            return result;
        }

        #endregion methods
        [System.Flags]
        private enum TaskCheckState
        {
            NotExist = 1,
            Started = 2,
            Completed = 4,
            Expired = 8
        }
    }
}