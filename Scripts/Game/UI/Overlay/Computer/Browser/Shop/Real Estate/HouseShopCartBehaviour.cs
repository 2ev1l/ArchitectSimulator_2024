using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class HouseShopCartBehaviour : RealEstateShopCartBehaviour<HouseShopItemData, RentableHouse>
    {
        #region fields & properties
        public override ShopData<HouseShopItemData> Data => GameData.Data.BrowserData.HouseShop;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}