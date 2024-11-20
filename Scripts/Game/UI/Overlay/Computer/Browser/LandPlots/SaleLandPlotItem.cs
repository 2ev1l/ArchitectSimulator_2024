using Game.Serialization.World;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.LandPlots
{
    public class SaleLandPlotItem : SellingLandPlotDataItem
    {
        #region fields & properties
        [SerializeField] private CustomButton withdrawButton;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            withdrawButton.OnClicked += Withdraw;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            withdrawButton.OnClicked -= Withdraw;
        }
        private void Withdraw()
        {
            GameData.Data.CompanyData.LandPlotsData.TryEndSelling(Context.Plot.Id);
        }
        #endregion methods
    }
}