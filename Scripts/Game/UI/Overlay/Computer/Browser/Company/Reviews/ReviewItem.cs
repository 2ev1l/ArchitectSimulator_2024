using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class ReviewItem : ContextActionsItem<ReviewData>
    {
        #region fields & properties
        [SerializeField] private HumanItem humanItem;
        [SerializeField] private Slider ratingSlider;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI monthText;
        [SerializeField] private TextMeshProUGUI blueprintNameText;
        [SerializeField] private Image positiveIndicator;
        [SerializeField] private Color positiveColor = Color.white;
        [SerializeField] private Color negativeColor = Color.white;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            ratingSlider.value = Context.Rating;
            descriptionText.text = Context.GetDescription();
            int monthLeft = GameData.Data.PlayerData.MonthData.CurrentMonth - Context.MonthCreated;
            monthText.text = $"{monthLeft} m.";
            blueprintNameText.text = Context.Task.ConstructionReference.Name;
            positiveIndicator.color = Context.IsPositive ? positiveColor : negativeColor;
        }
        public override void OnListUpdate(ReviewData param)
        {
            base.OnListUpdate(param);
            humanItem.OnListUpdate(param.Task.HumanInfo);
        }
        #endregion methods
    }
}