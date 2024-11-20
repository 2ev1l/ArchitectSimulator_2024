using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableFoodShopCartBehaviour : VirtualShopCartBehaviour<FoodShopItemData>
    {
        #region fields & properties
        public override ShopData<FoodShopItemData> Data => GameData.Data.BrowserData.FoodShop;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}