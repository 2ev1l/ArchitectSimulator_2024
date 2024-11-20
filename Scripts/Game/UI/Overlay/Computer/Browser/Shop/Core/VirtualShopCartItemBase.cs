using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class VirtualShopCartItemBase<T> : ContextActionsItem<VirtualShopItemContext<T>> where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        protected ShopCartData<T> ShopCartData => (ShopCartData<T>)Context.ShopData;
        protected VirtualShopItemBase<T> ShopItem => shopItem;
        [SerializeField] private VirtualShopItemBase<T> shopItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(VirtualShopItemContext<T> param)
        {
            base.OnListUpdate(param);
            shopItem.OnListUpdate(param);
        }
        #endregion methods
    }
}