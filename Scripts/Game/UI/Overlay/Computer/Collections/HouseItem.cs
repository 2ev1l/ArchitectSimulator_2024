using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class HouseItem : RealEstateItem
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI maximumPeopleText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            HouseInfo info = (HouseInfo)Context;
            if (maximumPeopleText != null)
                maximumPeopleText.text = $"{info.MaxPeople}x";
        }
        #endregion methods
    }
}