using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    public class FoodItem : DBItem<FoodInfo>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image previewImage;
        [SerializeField] private TextMeshProUGUI moodChangeText;
        [SerializeField] private TextMeshProUGUI saturationText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (nameText != null)
                nameText.text = Context.NameInfo.Text;
            if (previewImage != null)
                previewImage.sprite = Context.PreviewSprite;
            if (moodChangeText != null)
                moodChangeText.text = $"{(Context.MoodChange > 0 ? "+" : "")}{Context.MoodChange}%";
            if (saturationText != null)
                saturationText.text = $"+{Context.Saturation}%";
        }
        #endregion methods
    }
}