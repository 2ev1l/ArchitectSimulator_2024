using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class DBShopItem<ShopItem, BuyableObject, Info> : VirtualShopItem<ShopItem>
        where ShopItem : BuyableObjectItemData<BuyableObject>, ICloneable<ShopItem>
        where BuyableObject : DataBase.BuyableObject
        where Info : DBInfo
    {
        #region fields & properties
        public Info DBInfo => GetDBInfo(Context.ItemData.Item.Info);
        [SerializeField] private DBItem<Info> dbItem;
        #endregion fields & properties

        #region methods
        protected abstract Info GetDBInfo(BuyableObject buyableObjectContext);
        public override void OnListUpdate(VirtualShopItemContext<ShopItem> param)
        {
            base.OnListUpdate(param);
            dbItem.OnListUpdate(DBInfo);
        }
        #endregion methods
    }
}