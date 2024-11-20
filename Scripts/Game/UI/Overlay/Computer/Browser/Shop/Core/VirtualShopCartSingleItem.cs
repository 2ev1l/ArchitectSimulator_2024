using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class VirtualShopCartSingleItem<T> : VirtualShopCartItemBase<T> where T : ShopItemData, ISingleShopItem, ICloneable<T>
    {
        #region fields & properties
        [SerializeField] private GameObject soldOutUI;
        [SerializeField] private CustomButton buttonBuy;

        protected InfoRequest BadPurchaseRequest
        {
            get
            {
                badPurchaseRequest ??= new(null, $"{LanguageLoader.GetTextByType(TextType.Game, 38)}", $"{LanguageLoader.GetTextByType(TextType.Game, 37)}");
                return badPurchaseRequest;
            }
        }
        private InfoRequest badPurchaseRequest = null;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Updates UI
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateUI();
        }
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            buttonBuy.OnClicked += TryPurchase;
            ShopCartData.Cart.OnCartBought += UpdateUI;
            //don't subscribe on cart bought, because this action anyway invokes wallet change
            GameData.Data.PlayerData.Wallet.OnValueChanged += UpdateUI;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            buttonBuy.OnClicked -= TryPurchase;
            GameData.Data.PlayerData.Wallet.OnValueChanged -= UpdateUI;
        }
        private void UpdateUI(int _1, int _2) => UpdateUI();
        protected override void UpdateUI()
        {
            if (Context == null) return;
            base.UpdateUI();
            bool isSoldOut = IsSoldOut();
            bool canBuy = CanBuy();
            GameObject buttonObj = buttonBuy.gameObject;
            bool buttonActive = !isSoldOut && canBuy;
            if (buttonObj.activeSelf != buttonActive)
                buttonObj.SetActive(buttonActive);
            if (soldOutUI.activeSelf != isSoldOut)
                soldOutUI.SetActive(isSoldOut);
        }
        protected virtual bool IsSoldOut() => Context.ItemData.Item.IsOwned;
        /// <summary>
        /// Invokes before adding to cart
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanBuy()
        {
            if (!GameData.Data.PlayerData.Wallet.CanDecreaseValue(Context.ItemData.Item.FinalPrice)) return false;
            return true;
        }
        private void TryPurchase()
        {
            if (!CanBuy())
            {
                BadPurchaseRequest.Send();
            }
            Purchase();
        }
        protected virtual void Purchase()
        {
            ShopCartData.Cart.Add(Context.ItemData.Item, 1);
            if (!ShopCartData.Cart.CanPurchaseCart())
            {
                BadPurchaseRequest.Send();
                ShopCartData.Cart.Clear();
                return;
            }
            ShopItem.Context.ItemData.Item.MakeOwned();
            ShopCartData.Cart.PurchaseCart();
        }
        #endregion methods
    }
}