using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    public class RealEstateItem : DBItem<RealEstateInfo>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image previewImage;
        [SerializeField] private TextMeshProUGUI moodIncreaseText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (nameText != null)
                nameText.text = Context.NameInfo.Text;
            if (previewImage != null)
                previewImage.sprite = Context.PreviewSprite;
            if (moodIncreaseText != null)
                moodIncreaseText.text = $"{(Context.MoodScale >= 1 ? "+" : "")}{(Context.MoodScale - 1) * 100:F0}%";
        }
        #endregion methods
    }
}