using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class DescriptionUnfinishedBlueprintTaskItemList : DescriptionUnfinishedBlueprintItemList
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskRejected += UpdateListData;
            tasks.OnTaskAccepted += UpdateListData;
            tasks.OnTaskCompleted += UpdateListData;
            tasks.OnTaskStarted += UpdateListData;
            tasks.OnTaskExpired += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskRejected -= UpdateListData;
            tasks.OnTaskAccepted -= UpdateListData;
            tasks.OnTaskCompleted -= UpdateListData;
            tasks.OnTaskStarted -= UpdateListData;
            tasks.OnTaskExpired -= UpdateListData;
        }
        private void UpdateListData(ConstructionTaskData _) => UpdateListData();

        protected override void UpdateCurrentItems(List<BlueprintData> currentItemsReference)
        {
            currentItemsReference.Clear();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            BlueprintsData blueprints = GameData.Data.BlueprintsData;

            IReadOnlyList<ConstructionTaskData> startedTasks = tasks.StartedTasks;
            int startedTasksCount = startedTasks.Count;
            for (int i = 0; i < startedTasksCount; ++i)
            {
                ConstructionTaskData startedTask = startedTasks[i];
                if (!blueprints.TryGetByBaseId(startedTask.BlueprintBaseIdReference, out BlueprintData bp)) continue;
                currentItemsReference.Add(bp);
            }
        }
        #endregion methods
    }
}