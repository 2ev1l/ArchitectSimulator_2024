using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class StartedTaskConstructionItemList : StartedConstructionItemList
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskStarted += UpdateListData;
            tasks.OnTaskCompleted += UpdateListData;
            tasks.OnTaskExpired += UpdateListData;
            tasks.OnTaskAccepted += UpdateListData;
            tasks.OnTaskRejected += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskStarted -= UpdateListData;
            tasks.OnTaskCompleted -= UpdateListData;
            tasks.OnTaskExpired -= UpdateListData;
            tasks.OnTaskAccepted -= UpdateListData;
            tasks.OnTaskRejected -= UpdateListData;
        }
        private void UpdateListData(ConstructionTaskData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<ConstructionData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<ConstructionTaskData> tasks = GameData.Data.CompanyData.ConstructionTasks.StartedTasks;
            int count = tasks.Count;

            for (int i = 0; i < count; ++i)
            {
                ConstructionTaskData task = tasks[i];
                ConstructionData construction = task.ConstructionReference;
                if (construction == null) continue;
                if (construction.IsBuilded) continue;
                if (construction.BuildCompletionMonth < 0) continue;
                currentItemsReference.Add(construction);
            }
        }
        #endregion methods
    }
}