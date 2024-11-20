using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableVehicleShopCartItem : ChangableInfoBuyableShopCartItem<VehicleShopItemData, BuyableVehicle, VehicleInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<VehicleInfo> GetChangableInfo()
        {
            return GameData.Data.PlayerData.VehicleData;
        }
        protected override VehicleInfo GetDBInfo(BuyableVehicle buyableContext)
        {
            return buyableContext.Info;
        }
        #endregion methods
    }
}