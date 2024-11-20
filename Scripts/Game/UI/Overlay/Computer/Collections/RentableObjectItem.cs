using Game.DataBase;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public abstract class RentableObjectItem<T> : ContextActionsItem<T> where T : RentableObject
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI rentPriceText;
        [SerializeField] private TextMeshProUGUI purchasePriceText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            purchasePriceText.text = $"${Context.Price}";
            rentPriceText.text = $"${Context.RentPrice} / m.";
        }
        #endregion methods
    }
}