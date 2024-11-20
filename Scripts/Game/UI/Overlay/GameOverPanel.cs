using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Game.Events.GameOverChecker;

namespace Game.UI.Overlay
{
    public partial class GameOverPanel : PanelStateChange
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI endReasonText;
        #endregion fields & properties

        #region methods
        public void UpdateUI(EndReason endReason)
        {
            endReasonText.text = endReason.GetReason();
        }
        #endregion methods
    }
}