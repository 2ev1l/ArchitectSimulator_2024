using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class BuyableFoodShopCartItemList : ConstantShopItemsList<BuyableFoodShopCartItem, FoodShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.PlayerData.Food.OnSaturationIncreased += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.PlayerData.Food.OnSaturationIncreased -= UpdateListData;
        }
        private void UpdateListData(int _1, int _2) => UpdateListData();
        #endregion methods
    }
}