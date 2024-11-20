using Game.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay
{
    public class InfoOverlayPanel : OverlayPanel<InfoRequest>
    {
        #region fields & properties
        public TextMeshProUGUI HeaderText => headerText;
        [SerializeField] private TextMeshProUGUI headerText;
        public TextMeshProUGUI InfoText => infoText;
        [SerializeField] private TextMeshProUGUI infoText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI(InfoRequest request)
        {
            HeaderText.text = request.HeaderInfo;
            InfoText.text = request.MainInfo;
        }
        #endregion methods
    }
}