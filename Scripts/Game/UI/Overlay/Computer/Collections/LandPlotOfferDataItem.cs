using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class LandPlotOfferDataItem : ContextActionsItem<LandPlotOfferData>
    {
        #region fields & properties
        [SerializeField] private HumanItem humanItem;
        [SerializeField] private SellingLandPlotDataItem sellingLandPlotDataItem;
        [SerializeField] private TextMeshProUGUI offerDescriptionText;
        [SerializeField] private TextMeshProUGUI offerPriceText;
        [SerializeField] private CustomButton confirmOfferButton;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            confirmOfferButton.OnClicked += ConfirmOffer;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            confirmOfferButton.OnClicked -= ConfirmOffer;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            offerDescriptionText.text = Context.Description;
            offerPriceText.text = $"${Context.SellingPrice}";
        }
        private void ConfirmOffer() => GameData.Data.CompanyData.LandPlotsData.ConfirmOffer(Context);
        public override void OnListUpdate(LandPlotOfferData param)
        {
            base.OnListUpdate(param);
            humanItem.OnListUpdate(param.HumanInfo);
            sellingLandPlotDataItem.OnListUpdate(param.SellingPlot);
        }
        #endregion methods
    }
}