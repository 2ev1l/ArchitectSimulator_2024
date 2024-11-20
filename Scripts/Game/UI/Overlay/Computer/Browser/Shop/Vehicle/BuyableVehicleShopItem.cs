using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableVehicleShopItem : DBShopItem<VehicleShopItemData, BuyableVehicle, VehicleInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override VehicleInfo GetDBInfo(BuyableVehicle buyableObjectContext)
        {
            return buyableObjectContext.Info;
        }
        #endregion methods
    }
}