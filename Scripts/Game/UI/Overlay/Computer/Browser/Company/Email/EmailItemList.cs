using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class EmailItemList : InfinityFilteredItemListBase<EmailItem, ConstructionTaskData>
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
        protected override void UpdateCurrentItems(List<ConstructionTaskData> currentItemsReference)
        {
            currentItemsReference.Clear();

            IReadOnlyList<ConstructionTaskData> startedTasks = GameData.Data.CompanyData.ConstructionTasks.StartedTasks;
            int itemsCount = startedTasks.Count;
            for (int i = 0; i < itemsCount; ++i)
            {
                currentItemsReference.Add(startedTasks[i]);
            }
        }
        #endregion methods
    }
}