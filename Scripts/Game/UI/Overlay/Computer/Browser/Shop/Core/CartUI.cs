using EditorCustom.Attributes;
using Game.Serialization.World;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class CartUI<T> : MonoBehaviour where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        protected VirtualShopCartBehaviour<T> Shop => shop;
        [SerializeField] private VirtualShopCartBehaviour<T> shop;
        [Title("UI")]
        [SerializeField] private TextMeshProUGUI discountText;
        [SerializeField] private TextMeshProUGUI estimatedTotalText;
        [SerializeField] private TextMeshProUGUI summaryTotalText;
        [SerializeField] private TextMeshProUGUI summaryCountText;
        protected CustomButton PurchaseButton => purchaseButton;
        [SerializeField] private CustomButton purchaseButton;
        protected Color NormalColor => normalColor;
        [SerializeField] private Color normalColor = Color.white;
        protected Color BadColor => badColor;
        [SerializeField] private Color badColor = Color.red;

        private bool isSubscribed = false;

        private int calculatedDiscount = 0;
        private int calculatedCount = 0;
        #endregion fields & properties

        #region methods
        protected virtual void OnEnable()
        {
            Subscribe();
            UpdateUI();
        }
        protected virtual void OnDisable()
        {
            Unsubscribe();
        }
        private void Purchase()
        {
            shop.CartData.PurchaseCart();
            UpdateUI();
        }
        private void Subscribe()
        {
            if (isSubscribed) return;
            isSubscribed = true;

            purchaseButton.OnClicked += Purchase;
            shop.CartData.OnItemAdded += SubscribeAtItem;
            shop.CartData.OnItemRemoved += UnSubscribeAtItem;
            shop.Data.OnDataChanged += UpdateUI;
            ForEachCartItem(SubscribeAtItem);
        }
        private void Unsubscribe()
        {
            if (!isSubscribed) return;
            isSubscribed = false;

            purchaseButton.OnClicked -= Purchase;
            shop.CartData.OnItemAdded -= SubscribeAtItem;
            shop.CartData.OnItemRemoved -= UnSubscribeAtItem;
            shop.Data.OnDataChanged -= UpdateUI;
            ForEachCartItem(UnSubscribeAtItem);
        }
        protected void SubscribeAtItem(CountableItem<T> item)
        {
            item.OnCountChanged += UpdateUI;
        }
        protected void UnSubscribeAtItem(CountableItem<T> item)
        {
            if (item == null) return;
            item.OnCountChanged -= UpdateUI;
        }
        protected void ForEachCartItem(System.Action<CountableItem<T>> action)
        {
            int totalCount = shop.CartData.Items.Count;
            for (int i = 0; i < totalCount; ++i)
            {
                action.Invoke(shop.CartData.Items[i]);
            }
        }
        private void UpdateUI(int _) => UpdateUI();
        protected virtual void UpdateUI()
        {
            ResetItemsStats();
            ForEachCartItem(OnCalculateEachItemStats);
            int calculatedPrice = Shop.CartData.GetCartSum();
            bool canBuy = GameData.Data.PlayerData.Wallet.CanDecreaseValue(calculatedPrice);
            purchaseButton.enabled = canBuy;
            estimatedTotalText.color = canBuy ? normalColor : badColor;
            discountText.text = $"- ${Mathf.Abs(calculatedDiscount)}";
            estimatedTotalText.text = $"${calculatedPrice}";
            summaryTotalText.text = estimatedTotalText.text;
            summaryCountText.text = $"{calculatedCount}x";
        }
        protected virtual void ResetItemsStats()
        {
            calculatedDiscount = 0;
            calculatedCount = 0;
        }
        protected virtual void OnCalculateEachItemStats(CountableItem<T> x)
        {
            calculatedDiscount += (x.Item.FinalPrice - x.Item.StartPrice) * x.Count;
            calculatedCount += x.Count;
        }
        #endregion methods
    }
}