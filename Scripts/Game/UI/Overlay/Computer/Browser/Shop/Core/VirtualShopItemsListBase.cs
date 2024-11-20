using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Collections.Generic.Filters;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class VirtualShopItemsListBase<ShopItemBase, UpdateValue> : InfinityFilteredItemListBase<ShopItemBase, VirtualShopItemContext<UpdateValue>> 
        where UpdateValue : ShopItemData, ICloneable<UpdateValue>
        where ShopItemBase : ContextActionsItem<VirtualShopItemContext<UpdateValue>>
    {
        #region fields & properties
        protected VirtualShopBehaviour<UpdateValue> Shop => shop;
        [SerializeField] private VirtualShopBehaviour<UpdateValue> shop;
        [SerializeField] private VirtualFilters<VirtualShopItemContext<UpdateValue>, ShopItemData> shopItemDataFilters = new(x => x.ItemData.Item);
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            shop.Data.OnDataChanged += UpdateListData;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            shop.Data.OnDataChanged -= UpdateListData;
            base.OnDisable();
        }
        protected override IEnumerable<VirtualShopItemContext<UpdateValue>> GetFilteredItems(IEnumerable<VirtualShopItemContext<UpdateValue>> currentItems)
        {
            currentItems = shopItemDataFilters.ApplyFilters(currentItems);
            return base.GetFilteredItems(currentItems);
        }
        protected override void UpdateCurrentItems(List<VirtualShopItemContext<UpdateValue>> currentItemsReference)
        {
            currentItemsReference.Clear();
            foreach (UpdateValue el in shop.Data.Items)
            {
                currentItemsReference.Add(new(shop.Data, el));
            }
        }
        protected virtual void OnValidate()
        {
            shopItemDataFilters.Validate(this);
        }
        #endregion methods
    }
}