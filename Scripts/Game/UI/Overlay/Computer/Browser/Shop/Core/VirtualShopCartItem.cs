using EditorCustom.Attributes;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class VirtualShopCartItem<T> : VirtualShopCartItemBase<T> where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        [Title("Cart")]
        [SerializeField] private TMP_InputField countInputField;
        [SerializeField] private CustomButton removeFromCartButton;
        [SerializeField] private CustomButton addToCartButton;
        [SerializeField] private bool resetCountOnUpdate = false;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            if (addToCartButton != null)
                addToCartButton.OnClicked += AddToCart;
            if (removeFromCartButton != null)
                removeFromCartButton.OnClicked += RemoveFromCart;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            if (addToCartButton != null)
                addToCartButton.OnClicked -= AddToCart;
            if (removeFromCartButton != null)
                removeFromCartButton.OnClicked -= RemoveFromCart;
        }
        public void RemoveFromCart()
        {
            if (!TryGetInputCount(out int count)) return;
            ShopCartData.Cart.Remove(Context.ItemData.Item, count);
        }
        public void AddToCart()
        {
            if (!TryGetInputCount(out int count)) return;
            ShopCartData.Cart.Add(Context.ItemData.Item, count);
        }

        private bool TryGetInputCount(out int count)
        {
            count = 1;
            if (countInputField == null) return true;
            count = System.Convert.ToInt32(countInputField.text);
            if (count <= 0) return false;
            return true;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (resetCountOnUpdate && countInputField != null)
                countInputField.text = "1";
        }
        #endregion methods
    }
}