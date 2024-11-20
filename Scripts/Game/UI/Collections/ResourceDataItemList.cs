using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Collections.Generic.Filters;

namespace Game.UI.Collections
{
    public abstract class ResourceDataItemList<Resource> : InfinityFilteredItemListBase<ContextActionsItem<Resource>, Resource>
        where Resource : ResourceData
    {
        #region fields & properties
        protected WarehouseData WarehouseData => GameData.Data.CompanyData.WarehouseData;
        protected abstract IReadOnlyResources<Resource> Resources { get; }
        [SerializeField] private VirtualFilters<Resource, ResourceInfo> resourceFilters = new(x => x.Info);
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            Resources.OnResourceRemoved += UpdateListData;
            Resources.OnResourceAdded += UpdateListData;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Resources.OnResourceRemoved -= UpdateListData;
            Resources.OnResourceAdded -= UpdateListData;
        }
        private void UpdateListData(ResourceData _) => UpdateListData();
        protected override IEnumerable<Resource> GetFilteredItems(IEnumerable<Resource> currentItems)
        {
            currentItems = resourceFilters.ApplyFilters(currentItems);
            return base.GetFilteredItems(currentItems);
        }
        protected override void UpdateCurrentItems(List<Resource> currentItemsReference)
        {
            currentItemsReference.Clear();

            foreach (Resource el in Resources.Items)
            {
                currentItemsReference.Add(el);
            }
        }
        protected virtual void OnValidate()
        {
            resourceFilters.Validate(this);
        }
        #endregion methods
    }
}