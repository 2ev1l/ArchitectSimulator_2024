using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RentableLandPlotShopCartBehaviour : VirtualShopCartBehaviour<RentableLandPlotShopItemData>
    {
        #region fields & properties
        public override ShopData<RentableLandPlotShopItemData> Data => GameData.Data.BrowserData.LandPlotShop;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}