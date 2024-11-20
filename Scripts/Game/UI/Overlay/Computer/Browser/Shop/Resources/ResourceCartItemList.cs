using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ResourceCartItemList<ShopItem, Resource> : VirtualCartItemsList<ResourceCartItem<ShopItem, Resource>, ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override bool BaseFilterForItem(CountableItem<ShopItem> item)
        {
            return true;
        }
        public override void UpdateListData()
        {
            base.UpdateListData();
        }
        #endregion methods
    }
}