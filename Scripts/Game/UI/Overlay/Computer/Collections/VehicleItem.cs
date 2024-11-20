using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    public class VehicleItem : DBItem<VehicleInfo>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image previewImage;
        [SerializeField] private TextMeshProUGUI moodIncreaseText;
        [SerializeField] private TextMeshProUGUI maxSpeedText;
        [SerializeField] private TextMeshProUGUI time0100Text;
        [SerializeField] private TextMeshProUGUI releaseYearText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (nameText != null)
                nameText.text = Context.Name;
            if (previewImage != null)
                previewImage.sprite = Context.PreviewSprite;
            if (moodIncreaseText != null)
                moodIncreaseText.text = $"{(Context.MoodScale >= 1 ? "+" : "")}{(Context.MoodScale - 1) * 100:F0}%";
            if (maxSpeedText != null)
                maxSpeedText.text = $"{Context.MaxSpeed} km/h";
            if (time0100Text != null)
            {
                time0100Text.text = Context.Time0_100 < 99 ? $"{Context.Time0_100:F2} s." : "???";
            }
            if (releaseYearText != null)
                releaseYearText.text = $"{Context.ReleaseYear}";
        }
        #endregion methods
    }
}