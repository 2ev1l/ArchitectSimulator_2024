using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Behaviour;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class VirtualCartItemsList<ShopItemBase, UpdateValue> : VirtualShopItemsListBase<ShopItemBase, UpdateValue>
        where UpdateValue : ShopItemData, ICloneable<UpdateValue>
        where ShopItemBase : ContextActionsItem<VirtualShopItemContext<UpdateValue>>
    {
        #region fields & properties
        private List<VirtualShopItemContext<UpdateValue>> CartList
        {
            get
            {
                cartList.Clear();
                int totalCount = ShopCart.CartData.Items.Count;
                for (int i = 0; i < totalCount; ++i)
                {
                    CountableItem<UpdateValue> item = ShopCart.CartData.Items[i];
                    if (!BaseFilterForItem(item)) continue;
                    cartList.Add(new(ShopCart.Data, item));
                }
                return cartList;
            }
        }
        private List<VirtualShopItemContext<UpdateValue>> cartList = new();
        public VirtualShopCartBehaviour<UpdateValue> ShopCart => (VirtualShopCartBehaviour<UpdateValue>)base.Shop;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            ShopCart.CartData.OnItemRemoved += UpdateListData;
            ShopCart.CartData.OnItemAdded += UpdateListData;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            ShopCart.CartData.OnItemRemoved -= UpdateListData;
            ShopCart.CartData.OnItemAdded -= UpdateListData;
            base.OnDisable();
        }
        protected abstract bool BaseFilterForItem(CountableItem<UpdateValue> item);
        private void UpdateListData(CountableItem<UpdateValue> _) => UpdateListData();
        public override void UpdateListData()
        {
            ItemList.UpdateListDefault(CartList, x => x);
        }
        #endregion methods
    }
}