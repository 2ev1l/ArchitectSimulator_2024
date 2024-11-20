using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Behaviour;
using Universal.Collections.Filters;
using Universal.Collections.Generic.Filters;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ConstructionResourceShopItemList : ResourceShopCartItemList<ConstructionResourceShopItemData, BuyableConstructionResource>
    {
        #region fields & properties
        [SerializeField] private VirtualFilters<VirtualShopItemContext<ConstructionResourceShopItemData>, ConstructionResourceInfo> constructionResourceFilters = new(x => (ConstructionResourceInfo)x.ItemData.Item.Info.ResourceInfo);
        [SerializeField] private VirtualPageItemFilter<VirtualShopItemContext<ConstructionResourceShopItemData>> pageItemFilter = new();
        [SerializeField] private DefaultStateMachine constructionTypeStateMachine;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            constructionTypeStateMachine.Context.OnStateChanged += OnStateMachineChanged;
            pageItemFilter.OnUpdateRequested += UpdateListDataWithFiltersOnly;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            constructionTypeStateMachine.Context.OnStateChanged -= OnStateMachineChanged;
            pageItemFilter.OnUpdateRequested -= UpdateListDataWithFiltersOnly;
        }
        protected override IEnumerable<VirtualShopItemContext<ConstructionResourceShopItemData>> GetFilteredItems(IEnumerable<VirtualShopItemContext<ConstructionResourceShopItemData>> currentItems)
        {
            ConstructionType constructionType = (ConstructionType)constructionTypeStateMachine.Context.CurrentStateId;
            currentItems = currentItems.Where(x => ((ConstructionResourceInfo)x.ItemData.Item.Info.ResourceInfo).ConstructionType == constructionType);
            currentItems = constructionResourceFilters.ApplyFilters(currentItems);
            return pageItemFilter.ApplyFilters(base.GetFilteredItems(currentItems));
        }
        private void OnStateMachineChanged(StateChange _)
        {
            UpdateListDataWithFiltersOnly();
        }
        protected override void OnValidate()
        {
            base.OnValidate();
            constructionResourceFilters.Validate(this);
        }
        #endregion methods
    }
}