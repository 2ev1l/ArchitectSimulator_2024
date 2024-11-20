using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class FoodCartData : SingleItemCartData<FoodShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool CanPurchaseCart()
        {
            if (!GameData.Data.PlayerData.Food.CanIncreaseSaturation) return false;
            return base.CanPurchaseCart();
        }
        #endregion methods
    }
}