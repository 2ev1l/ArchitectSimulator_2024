using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    [System.Serializable]
    public class ConstructionResourceShopCartBehaviour : ResourceShopCartBehaviour<ConstructionResourceShopItemData, BuyableConstructionResource>
    {
        #region fields & properties
        public override ShopData<ConstructionResourceShopItemData> Data => GameData.Data.BrowserData.ConstructionResourceShop;
        [SerializeField] private VirtualBrowser browser;
        [SerializeField] private Canvas cartCanvas;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            browser.OnViewFocusChangedAction += OnFocusChanged;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            browser.OnViewFocusChangedAction -= OnFocusChanged;
        }
        private void OnFocusChanged()
        {
            if (cartCanvas != null)
                cartCanvas.enabled = browser.IsMainVisibleApplication();
        }
        #endregion methods
    }
}