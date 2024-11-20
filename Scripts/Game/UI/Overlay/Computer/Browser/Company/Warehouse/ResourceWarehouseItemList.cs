using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Elements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Collections.Generic.Filters;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public abstract class ResourceWarehouseItemList<T> : InfinityFilteredItemListBase<ResourceWarehouseItem<T>, T>
        where T : ResourceData
    {
        #region fields & properties
        public abstract IReadOnlyResources<T> Resources { get; }
        [SerializeField] private VirtualFilters<T, ResourceInfo> resourceInfoFilters = new(x => x.Info);
        [SerializeField] private CustomButton filtersRefreshButton;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            Resources.OnResourceRemoved += UpdateListData;
            Resources.OnResourceAdded += UpdateListData;
            filtersRefreshButton.OnClicked += UpdateListDataWithFiltersOnly;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Resources.OnResourceRemoved -= UpdateListData;
            Resources.OnResourceAdded -= UpdateListData;
            filtersRefreshButton.OnClicked -= UpdateListDataWithFiltersOnly;
        }
        protected override IEnumerable<T> GetFilteredItems(IEnumerable<T> currentItems)
        {
            currentItems = resourceInfoFilters.ApplyFilters(currentItems);
            return base.GetFilteredItems(currentItems);
        }
        private void UpdateListData(ResourceData _) => UpdateListData();

        public override void UpdateListData()
        {
            base.UpdateListData();
        }
        protected virtual void OnValidate()
        {
            resourceInfoFilters.Validate(this);
        }
        #endregion methods
    }
}