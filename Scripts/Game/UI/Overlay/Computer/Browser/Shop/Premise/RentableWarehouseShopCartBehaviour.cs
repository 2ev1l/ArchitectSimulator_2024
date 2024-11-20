using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    [System.Serializable]
    public class RentableWarehouseShopCartBehaviour : RentablePremiseShopCartBehaviour<RentableWarehouseShopItemData, RentableWarehouse>
    {
        #region fields & properties
        public override ShopData<RentableWarehouseShopItemData> Data => GameData.Data.BrowserData.WarehouseShop;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}