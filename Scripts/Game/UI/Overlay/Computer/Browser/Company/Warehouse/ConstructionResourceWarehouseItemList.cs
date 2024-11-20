using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Filters;
using Universal.Collections.Generic.Filters;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class ConstructionResourceWarehouseItemList : ResourceWarehouseItemList<ConstructionResourceData>
    {
        #region fields & properties
        public override IReadOnlyResources<ConstructionResourceData> Resources => GameData.Data.CompanyData.WarehouseData.ConstructionResources;
        [SerializeField] private VirtualFilters<ConstructionResourceData, ConstructionResourceInfo> constructionResourceInfoFilters = new(x => (ConstructionResourceInfo)x.Info);
        [SerializeField] private VirtualPageItemFilter<ConstructionResourceData> pageItemFilter;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            pageItemFilter.OnUpdateRequested += UpdateListDataWithFiltersOnly;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            pageItemFilter.OnUpdateRequested -= UpdateListDataWithFiltersOnly;
        }
        protected override IEnumerable<ConstructionResourceData> GetFilteredItems(IEnumerable<ConstructionResourceData> currentItems)
        {
            currentItems = constructionResourceInfoFilters.ApplyFilters(currentItems);
            return pageItemFilter.ApplyFilters(base.GetFilteredItems(currentItems));
        }
        protected override void UpdateCurrentItems(List<ConstructionResourceData> currentItemsReference)
        {
            currentItemsReference.Clear();
            var resources = Resources;
            int totalCount = resources.Items.Count;
            for (int i = 0; i < totalCount; ++i)
            {
                currentItemsReference.Add(resources.Items[i]);
            }
        }
        protected override void OnValidate()
        {
            base.OnValidate();
            constructionResourceInfoFilters.Validate(this);
        }
        #endregion methods
    }
}