using Game.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Universal.Collections;

namespace Game.UI.Overlay
{
    public class PopupStatsContent : DestroyablePoolableObject
    {
        #region fields & properties
        [SerializeField] private Image indicator;
        [SerializeField] private TextMeshProUGUI text;
        #endregion fields & properties

        #region methods
        public virtual void UpdateUI(PopupRequest popupRequest)
        {
            indicator.sprite = popupRequest.IndicatorSprite;
            Color color = popupRequest.GetColor();
            indicator.color = color;
            text.color = color;
            text.text = popupRequest.GetText();
        }
        #endregion methods
    }
}