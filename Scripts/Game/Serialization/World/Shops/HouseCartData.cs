using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class HouseCartData : RealEstateCartData<HouseShopItemData, RentableHouse>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<RealEstateInfo> GetChangableInfo()
        {
            return GameData.Data.PlayerData.HouseData;
        }
        #endregion methods
    }
}