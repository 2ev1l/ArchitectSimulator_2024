using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class ChangableInfoBuyableShopCartItem<ShopItem, Buyable, Info> : DBShopCartItem<ShopItem, Buyable, Info>
        where ShopItem : BuyableObjectItemData<Buyable>, ICloneable<ShopItem>, ISingleShopItem
        where Buyable : BuyableObject
        where Info : DBInfo
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override bool CanBuy()
        {
            if (!GetChangableInfo().CanReplaceInfo(DBInfo.Id)) return false;
            return base.CanBuy();
        }
        protected override bool IsSoldOut()
        {
            if (GetChangableInfo().Id == DBInfo.Id) return true;
            return base.IsSoldOut();
        }
        protected abstract ChangableInfoData<Info> GetChangableInfo();
        #endregion methods
    }
}