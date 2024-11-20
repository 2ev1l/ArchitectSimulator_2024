using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class AvailableBlueprintTaskItemList : AvailableBlueprintItemList<AvailableBlueprintTaskItem>
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
            tasks.OnTaskAccepted += UpdateListData; 
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.OnTaskRejected -= UpdateListData;
            tasks.OnTaskCompleted -= UpdateListData;
            tasks.OnTaskStarted -= UpdateListData;
            tasks.OnTaskExpired -= UpdateListData;
            tasks.OnTaskAccepted -= UpdateListData;
        }
        private void UpdateListData(ConstructionTaskData _) => UpdateListData();

        protected override void UpdateCurrentItems(List<BlueprintInfo> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<ConstructionTaskData> startedTasks = GameData.Data.CompanyData.ConstructionTasks.StartedTasks;
            int tasksCount = startedTasks.Count;
            for (int i = 0; i < tasksCount; ++i)
            {
                ConstructionTaskData startedTask = startedTasks[i];
                if (startedTask.BlueprintBaseIdReference > -1) continue;
                BlueprintInfo blueprint = startedTask.Info.BlueprintInfo;
                currentItemsReference.Add(blueprint);
            }
        }
        #endregion methods
    }
}