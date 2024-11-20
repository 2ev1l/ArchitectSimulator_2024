using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Overlay
{
    public abstract class DescriptionTaskItemList<TaskData, Info> : DescriptionItemList<TaskData>
        where TaskData : TaskData<Info>
        where Info : TaskInfo
    {
        #region fields & properties
        protected abstract TasksData<TaskData, Info> TasksData { get; }
        [SerializeField] private bool updateCurrentTasks = false;
        [SerializeField] private bool updateCompletedTasks = false;
        [SerializeField] private bool updateExpiredTasks = false;
        #endregion fields & properties

        #region methods
        protected override void UpdateCurrentItems(List<TaskData> currentItemsReference)
        {
            currentItemsReference.Clear();
            if (updateCurrentTasks)
            {
                currentItemsReference.AddRange(TasksData.StartedTasks);
            }
            if (updateCompletedTasks)
            {
                currentItemsReference.AddRange(TasksData.CompletedTasks);
            }
            if (updateExpiredTasks)
            {
                currentItemsReference.AddRange(TasksData.ExpiredTasks);
            }
        }
        #endregion methods
    }
}