using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class ResourceCartItem<ShopItem, Resource> : VirtualShopCartItem<ShopItem>
        where ShopItem : ResourceShopItemData<Resource>, ICloneable<ShopItem>
        where Resource : BuyableResource
    {
        #region fields & properties
        [SerializeField] private ResourceShopItem<ShopItem, Resource> resourceShopItem;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TextMeshProUGUI totalPriceText;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            Context.ItemData.OnCountChanged += UpdateQuantityText;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            Context.ItemData.OnCountChanged -= UpdateQuantityText;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            UpdateQuantityText();
        }
        private void UpdateQuantityText(int _) => UpdateQuantityText();
        private void UpdateQuantityText()
        {
            quantityText.text = $"{Context.ItemData.Count}x";
            totalPriceText.text = $"${Context.ItemData.Count * Context.ItemData.Item.FinalPrice}";
        }
        public override void OnListUpdate(VirtualShopItemContext<ShopItem> param)
        {
            base.OnListUpdate(param);
            resourceShopItem.OnListUpdate(param);
        }
        #endregion methods
    }
}