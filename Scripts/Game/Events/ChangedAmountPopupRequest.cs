using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Events
{
    [System.Serializable]
    public class ChangedAmountPopupRequest : PopupRequest
    {
        #region fields & properties
        [SerializeField] private string valuePostfix;
        public int TotalAmount
        {
            get => totalAmount;
            set => totalAmount = value;
        }
        private int totalAmount;
        public int ChangedAmount
        {
            get => changedAmount;
            set => changedAmount = value;
        }
        private int changedAmount;
        private static Color NegativeColor
        {
            get
            {
                if (negativeColor == Color.black)
                    ColorUtility.TryParseHtmlString("#F4ADAD", out negativeColor);
                return negativeColor;
            }
        }
        private static Color negativeColor = Color.black;
        private static Color PositiveColor
        {
            get
            {
                if (positiveColor == Color.black)
                    ColorUtility.TryParseHtmlString("#ADD9F4", out positiveColor);
                return positiveColor;
            }
        }
        private static Color positiveColor = Color.black;
        private static readonly int digitsAllowed = 17;
        private static readonly int digitsLimit = 24;
        #endregion fields & properties

        #region methods
        public override string GetText()
        {
            int prevAmount = totalAmount - changedAmount;
            int totalDigits = totalAmount.DigitsCount() + prevAmount.DigitsCount() + changedAmount.DigitsCount();
            float lineHeight = Mathf.Clamp(Mathf.InverseLerp(digitsAllowed, digitsLimit, totalDigits), 0f, 1f);
            return $"{prevAmount}{valuePostfix} => {totalAmount}{valuePostfix}<line-height={(lineHeight).ToString().Replace(",", ".")}em>\n<align=right>{(changedAmount >= 0 ? "+" : "")}{changedAmount}{valuePostfix}";
        }

        public override Color GetColor()
        {
            return changedAmount >= 0 ? PositiveColor : NegativeColor;
        }
        #endregion methods
    }
}