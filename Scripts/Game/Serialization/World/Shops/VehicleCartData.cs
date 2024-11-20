using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class VehicleCartData : ChangableInfoCartData<VehicleShopItemData, BuyableVehicle, VehicleInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<VehicleInfo> GetChangableInfo()
        {
            return GameData.Data.PlayerData.VehicleData;
        }
        #endregion methods
    }
}