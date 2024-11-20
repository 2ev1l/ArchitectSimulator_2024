using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using Game.UI.Overlay.Computer.DesignApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Filters;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class CompletedConstructionItemList : InfinityFilteredItemListBase<CompletedConstructionItem, ConstructionData>
    {
        #region fields & properties
        [SerializeField] private ConstructionPreviewPanel constructionPreviewPanel;
        [SerializeField] private VirtualPageItemFilter<ConstructionData> pageFilter;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            constructionPreviewPanel.DisablePanel();
            ConstructionsData constructionsData = GameData.Data.ConstructionsData;
            constructionsData.OnConstructionBuilded += UpdateListData;
            constructionsData.OnConstructionBuildCanceled += UpdateListData;
            pageFilter.OnUpdateRequested += UpdateListDataWithFiltersOnly;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            constructionPreviewPanel.DisablePanel();
            ConstructionsData constructionsData = GameData.Data.ConstructionsData;
            constructionsData.OnConstructionBuilded -= UpdateListData;
            constructionsData.OnConstructionBuildCanceled -= UpdateListData;
            pageFilter.OnUpdateRequested -= UpdateListDataWithFiltersOnly;
        }
        protected override IEnumerable<ConstructionData> GetFilteredItems(IEnumerable<ConstructionData> currentItems)
        {
            return pageFilter.ApplyFilters(base.GetFilteredItems(currentItems));
        }
        private void UpdateListData(ConstructionData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<ConstructionData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<ConstructionData> constructions = GameData.Data.ConstructionsData.Constructions;
            int count = constructions.Count;
            for (int i = 0; i < count; ++i)
            {
                ConstructionData construction = constructions[i];
                if (!construction.IsBuilded) continue;
                currentItemsReference.Add(construction);
            }
        }
        #endregion methods
    }
}