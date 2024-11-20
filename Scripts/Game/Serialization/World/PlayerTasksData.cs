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
    public class PlayerTasksData : TasksData<PlayerTaskData, PlayerTaskInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnBeforeTaskCompleted(PlayerTaskData task)
        {
            base.OnBeforeTaskCompleted(task);
            foreach (int nextTaskId in task.Info.NextTasksTrigger)
            {
                TryStartTask(nextTaskId);
            }
        }
        protected override PlayerTaskData CreateNewTask(int id) => new(id);
        #endregion methods
    }
}