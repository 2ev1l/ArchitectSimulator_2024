using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class ChangableInfoRentableShopCartItem<ShopItem, Rentable, Info> : RentableObjectShopCartItem<ShopItem, Rentable, Info>
        where ShopItem : ChangableInfoRentableItemData<Rentable, Info>, ICloneable<ShopItem>
        where Rentable : RentableObject
        where Info : DBInfo, INameHandler
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