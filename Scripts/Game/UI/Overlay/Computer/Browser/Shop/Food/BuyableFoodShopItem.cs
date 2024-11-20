using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableFoodShopItem : DBShopItem<FoodShopItemData, BuyableFood, FoodInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override FoodInfo GetDBInfo(BuyableFood buyableObjectContext)
        {
            return buyableObjectContext.Info;
        }
        #endregion methods
    }
}