using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic.Filters;

namespace Game.UI.Collections
{
    [System.Serializable]
    public class ConstructionResourceDataItemList : ResourceDataItemList<ConstructionResourceData>
    {
        #region fields & properties
        protected override IReadOnlyResources<ConstructionResourceData> Resources => WarehouseData.ConstructionResources;
        [SerializeField] private VirtualFilters<ConstructionResourceData, ConstructionResourceInfo> constructionResourceFilters = new(x => (ConstructionResourceInfo)x.Info);
        #endregion fields & properties

        #region methods
        protected override IEnumerable<ConstructionResourceData> GetFilteredItems(IEnumerable<ConstructionResourceData> currentItems)
        {
            currentItems = constructionResourceFilters.ApplyFilters(currentItems);
            return base.GetFilteredItems(currentItems);
        }
        protected override void OnValidate()
        {
            constructionResourceFilters.Validate(this);
            base.OnValidate();
        }
        #endregion methods
    }
}