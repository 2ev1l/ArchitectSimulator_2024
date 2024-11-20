using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay
{
    public abstract class DescriptionPremiseInfoPanel<Info> : DescriptionPanel<Info> where Info : PremiseInfo
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI premiseNameText;
        [SerializeField] private Image previewImage;
        #endregion fields & properties

        #region methods
        protected override void OnUpdateUI()
        {
            premiseNameText.text = Data.NameInfo.Text;
            previewImage.sprite = Data.PreviewSprite;
        }
        #endregion methods
    }
}