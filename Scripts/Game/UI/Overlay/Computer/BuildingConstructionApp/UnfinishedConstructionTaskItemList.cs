using Game.Serialization.World;
using Game.UI.Overlay.Computer.BuildingConstructionApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class UnfinishedConstructionTaskItemList : UnfinishedConstructionItemList
{
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskRejected += UpdateListData;
            tasks.OnTaskCompleted += UpdateListData;
            tasks.OnTaskStarted += UpdateListData;
            tasks.OnTaskExpired += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskRejected -= UpdateListData;
            tasks.OnTaskCompleted -= UpdateListData;
            tasks.OnTaskStarted -= UpdateListData;
            tasks.OnTaskExpired -= UpdateListData;
        }
        private void UpdateListData(ConstructionTaskData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<ConstructionData> currentItemsReference)
        {
            currentItemsReference.Clear();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;

            IReadOnlyList<ConstructionTaskData> startedTasks = tasks.StartedTasks;
            int startedTasksCount = startedTasks.Count;
            for (int i = 0; i < startedTasksCount; ++i)
            {
                ConstructionTaskData startedTask = startedTasks[i];
                ConstructionData cd = startedTask.ConstructionReference;
                if (cd == null) continue;
                if (cd.BuildCompletionMonth > -1) continue;
                currentItemsReference.Add(cd);
            }
        }
        #endregion methods
    }
}