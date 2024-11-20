using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RentableObjectShopCartItemList<ShopItem, Rentable, Info> : ConstantShopItemsList<RentableObjectShopCartItem<ShopItem, Rentable, Info>, ShopItem>
        where ShopItem : RentableObjectItemData<Rentable>, ICloneable<ShopItem>
        where Rentable : RentableObject
        where Info : DBInfo, INameHandler
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}