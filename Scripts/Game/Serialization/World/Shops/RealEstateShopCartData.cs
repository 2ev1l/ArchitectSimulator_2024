using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RealEstateShopCartData<ShopItem, RealEstate> : ShopCartData<ShopItem>
        where ShopItem : RealEstateShopItemData<RealEstate>, ICloneable<ShopItem>
        where RealEstate : RentableRealEstate
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}