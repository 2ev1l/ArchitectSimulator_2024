using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RealEstateCartData<ShopItem, RealEstate> : ChangableInfoCartData<ShopItem, RealEstate, RealEstateInfo>
        where ShopItem : RealEstateShopItemData<RealEstate>, ICloneable<ShopItem>
        where RealEstate : RentableRealEstate
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}