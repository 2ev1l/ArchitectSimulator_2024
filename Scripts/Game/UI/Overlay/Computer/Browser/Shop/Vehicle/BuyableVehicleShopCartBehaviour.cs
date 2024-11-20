using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableVehicleShopCartBehaviour : VirtualShopCartBehaviour<VehicleShopItemData>
    {
        #region fields & properties
        public override ShopData<VehicleShopItemData> Data => GameData.Data.BrowserData.VehicleShop;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}