using Game.DataBase;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Collections
{
    public class HumanItem : DBItem<HumanInfo>
    {
        #region fields & properties
        [SerializeField] private Image faceImage;
        [SerializeField] private TextMeshProUGUI nameText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            faceImage.sprite = Context.PreviewSprite;
            nameText.text = Context.Name;
        }
        #endregion methods
    }
}