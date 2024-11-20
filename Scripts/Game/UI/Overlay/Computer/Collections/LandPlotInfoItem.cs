using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class LandPlotInfoItem : PremiseItem
    {
        #region fields & properties
        private LandPlotInfo LandPlotInfo => (LandPlotInfo)base.Context;
        [SerializeField] private TextMeshProUGUI allowedConstructionText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            BuildingInfo buildingInfo = LandPlotInfo.BlueprintInfo.BuildingInfo;
            if (allowedConstructionText != null)
                allowedConstructionText.text = $"{buildingInfo.BuildingType.ToLanguage()}-{buildingInfo.BuildingStyle.ToLanguage()}";
        }
        public void ChangePreviewSprite(bool isBuilded)
        {
            if (base.PreviewImage == null) return;
            base.PreviewImage.sprite = isBuilded ? LandPlotInfo.BuildedSprite : LandPlotInfo.PreviewSprite;
        }
        #endregion methods
    }
}