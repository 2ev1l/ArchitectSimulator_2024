using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.LandPlots
{
    public class OwnLandPlotItem : LandPlotDataItem
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI estimatedValueText;
        [SerializeField] private TMP_InputField priceInputField;
        [SerializeField] private CustomButton saleButton;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            saleButton.OnClicked += SaleItem;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            saleButton.OnClicked -= SaleItem;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            int maxPrice = Context.MaxSellPrice;
            int basePrice = Context.BaseSellPrice;
            if (maxPrice.Approximately(basePrice, 5))
            {
                estimatedValueText.text = $"${basePrice}";
            }
            else
            {
                estimatedValueText.text = $"${basePrice} ... ${maxPrice}";
            }
        }
        private int GetInputFieldPrice()
        {
            int price = 0;
            try
            {
                price = System.Convert.ToInt32(priceInputField.text);
            }
            catch { }
            if (price < 0) return 0;
            return price;
        }
        private void SaleItem()
        {
            GameData.Data.CompanyData.LandPlotsData.TryStartSelling(Context.Id, GetInputFieldPrice());
        }
        #endregion methods
    }
}