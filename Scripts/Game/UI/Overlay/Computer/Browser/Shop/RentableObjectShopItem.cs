using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RentableObjectShopItem<ShopItem, Rentable, Info> : DBShopItem<ShopItem, Rentable, Info>
        where ShopItem : RentableObjectItemData<Rentable>, ICloneable<ShopItem>
        where Rentable : RentableObject
        where Info : DBInfo
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI rentPriceText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            rentPriceText.text = $"${Context.ItemData.Item.Info.RentPrice} / m.";
        }
        #endregion methods
    }
}