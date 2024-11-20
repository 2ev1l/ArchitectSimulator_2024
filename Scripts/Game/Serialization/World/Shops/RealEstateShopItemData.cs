using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RealEstateShopItemData<RealEstate> : ChangableInfoRentableItemData<RealEstate, RealEstateInfo>
        where RealEstate : RentableRealEstate
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected RealEstateShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}