using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class VirtualShopItem<T> : VirtualShopItemBase<T> where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Color defaultPriceColor = Color.black;
        [SerializeField] private Color discountPriceColor = Color.red;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            string colorDefault = ColorUtility.ToHtmlStringRGB(defaultPriceColor);
            if (!Context.ItemData.Item.HasDiscount)
            {
                if (priceText != null)
                    priceText.text = $"<color=#{colorDefault}>${Context.ItemData.Item.StartPrice}</color>";
                return;
            }
            string colorDiscount = ColorUtility.ToHtmlStringRGB(discountPriceColor);
            if (priceText != null)
                priceText.text = $"<s><size=75%><color=#{colorDefault}>${Context.ItemData.Item.StartPrice}</color></size></s> " +
                    $"<color=#{colorDiscount}>${Context.ItemData.Item.FinalPrice}</color>";
        }
        #endregion methods
    }
}