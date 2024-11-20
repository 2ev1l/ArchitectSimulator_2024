using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class LandPlotDataItem : ContextActionsItem<LandPlotData>
    {
        #region fields & properties
        [SerializeField] private LandPlotInfoItem landPlotItem;
        [SerializeField] private TextMeshProUGUI occupacyText;
        private bool IsBuilded => Context.ConstructionReference != null && Context.ConstructionReference.IsBuilded;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (occupacyText != null)
                occupacyText.text = LanguageInfo.GetTextByType(TextType.Game, IsBuilded ? 286 : 285);
        }
        public override void OnListUpdate(LandPlotData param)
        {
            base.OnListUpdate(param);
            landPlotItem.OnListUpdate(param.PlotInfo);
            landPlotItem.ChangePreviewSprite(IsBuilded);
        }
        #endregion methods
    }
}