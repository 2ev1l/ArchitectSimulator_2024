using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableFoodShopCartItem : VirtualShopCartSingleItem<FoodShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override bool CanBuy()
        {
            if (!GameData.Data.PlayerData.Food.CanIncreaseSaturation) return false;
            return base.CanBuy();
        }
        #endregion methods
    }
}