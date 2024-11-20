using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class DBShopCartItem<ShopItem, Buyable, Info> : VirtualShopCartSingleItem<ShopItem>
        where ShopItem : BuyableObjectItemData<Buyable>, ICloneable<ShopItem>, ISingleShopItem
        where Buyable : BuyableObject
        where Info : DBInfo
    {
        #region fields & properties
        public Info DBInfo => GetDBInfo(Context.ItemData.Item.Info);
        #endregion fields & properties

        #region methods
        protected abstract Info GetDBInfo(Buyable buyableContext);
        #endregion methods
    }
}