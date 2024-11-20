using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ChangableInfoCartData<ShopItem, BuyableInfo, Info> : SingleItemCartData<ShopItem>
        where ShopItem : BuyableObjectItemData<BuyableInfo>, ICloneable<ShopItem>, ISingleShopItem
        where BuyableInfo : BuyableObject
        where Info : DBInfo
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool CanPurchaseCart()
        {
            if (!base.CanPurchaseCart()) return false;
            if (!GetChangableInfo().CanReplaceInfo(CartItem.Item.Info.Id)) return false;
            return true;
        }
        protected abstract ChangableInfoData<Info> GetChangableInfo();
        #endregion methods
    }
}