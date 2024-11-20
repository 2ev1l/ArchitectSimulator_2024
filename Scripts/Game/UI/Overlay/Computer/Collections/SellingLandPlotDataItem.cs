using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class SellingLandPlotDataItem : ContextActionsItem<SellingLandPlotData>
    {
        #region fields & properties
        [SerializeField] private LandPlotDataItem landPlotDataItem;
        [SerializeField] private TextMeshProUGUI priceText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            priceText.text = $"${Context.SellingPrice}";
        }
        public override void OnListUpdate(SellingLandPlotData param)
        {
            base.OnListUpdate(param);
            landPlotDataItem.OnListUpdate(param.Plot);
        }
        #endregion methods
    }
}