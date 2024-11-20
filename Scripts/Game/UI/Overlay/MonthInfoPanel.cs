using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay
{
    public class MonthInfoPanel : PanelStateChange
    {
        #region fields & properties
        private static readonly LanguageInfo monthlyResultsLanguage = new(162, TextType.Game);
        private static readonly LanguageInfo increaseMoneyLanguage = new(153, TextType.Game);
        private static readonly LanguageInfo decreaseMoneyLanguage = new(154, TextType.Game);
        private static readonly LanguageInfo increaseTimeLanguage = new(158, TextType.Game);
        private static readonly LanguageInfo decreaseTimeLanguage = new(159, TextType.Game);
        private static readonly LanguageInfo increaseMoodLanguage = new(160, TextType.Game);
        private static readonly LanguageInfo decreaseMoodLanguage = new(161, TextType.Game);
        private static readonly LanguageInfo increaseCompletedTasksLanguage = new(155, TextType.Game);
        private static readonly LanguageInfo zeroCompletedTasksLanguage = new(156, TextType.Game);
        private static readonly LanguageInfo billsPaymentSumLanguage = new(163, TextType.Game);
        private static readonly LanguageInfo billsPaymentZeroLanguage = new(164, TextType.Game);
        private static readonly LanguageInfo billsPaymentMonthLanguage = new(165, TextType.Game);
        private static readonly LanguageInfo billsPaymentMonthDangerousLanguage = new(166, TextType.Game);
        private static readonly LanguageInfo saturationGoodLanguage = new(391, TextType.Game);
        private static readonly LanguageInfo saturationNormalLanguage = new(392, TextType.Game);
        private static readonly LanguageInfo saturationBadLanguage = new(393, TextType.Game);
        private static readonly LanguageInfo saturationDangerousLanguage = new(394, TextType.Game);
        private static readonly LanguageInfo saturationLanguage = new(307, TextType.Game);

        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private Image background;
        [SerializeField] private Color neutralBackgroundColor = Color.white;
        [SerializeField] private Color goodBackgroundColor = Color.green;
        [SerializeField] private Color badBackgroundColor = Color.red;
        [SerializeField] private Color goodGainColor = Color.green;
        [SerializeField] private Color badGainColor = Color.red;
        #endregion fields & properties

        #region methods
        [Button(nameof(UpdateUI))]
        public void UpdateUI()
        {
            MonthData month = GameData.Data.PlayerData.MonthData;
            UpdateTextUI(month);
            UpdateBackgroundUI(month.GainStatistic);
        }
        private void UpdateTextUI(MonthData month)
        {
            PlayerData playerData = GameData.Data.PlayerData;
            string goodGainColor = ColorUtility.ToHtmlStringRGB(this.goodGainColor);
            string badGainColor = ColorUtility.ToHtmlStringRGB(this.badGainColor);
            string normalGainColor = ColorUtility.ToHtmlStringRGB(Color.white);
            StringBuilder sb = new();
            sb.Append($"<align=center><size=125%>{monthlyResultsLanguage.Text}</size></align><br>");
            AddLineBasedOnValue(sb, month.GainStatistic.Money, "Money", increaseMoneyLanguage.Text, decreaseMoneyLanguage.Text, "", goodGainColor, badGainColor);
            AddLineBasedOnValue(sb, month.GainStatistic.Time, "Hours", increaseTimeLanguage.Text, decreaseTimeLanguage.Text, "", goodGainColor, badGainColor);
            AddLineBasedOnValue(sb, month.GainStatistic.Mood, "Mood", increaseMoodLanguage.Text, decreaseMoodLanguage.Text, "", goodGainColor, badGainColor);
            AddLineBasedOnValue(sb, month.GainStatistic.CompletedTasks, "Tasks", increaseCompletedTasksLanguage.Text, "", zeroCompletedTasksLanguage.Text, goodGainColor, "");

            int billsPaymentAmount = playerData.BillsData.GetBillsSum();
            bool canPayBills = playerData.Wallet.CanDecreaseValue(billsPaymentAmount);
            billsPaymentAmount = -billsPaymentAmount;
            AddLineBasedOnValue(sb, billsPaymentAmount, "Money", "", billsPaymentSumLanguage.Text, billsPaymentZeroLanguage.Text, canPayBills ? goodGainColor : badGainColor, canPayBills ? goodGainColor : badGainColor);
            if (billsPaymentAmount != 0)
            {
                int monthLeftUntilPayment = playerData.BillsData.GetMinimalMonthUntilNextBill();
                if (!playerData.BillsData.HasUnpayedBills)
                {
                    sb.Append($"\n{billsPaymentMonthLanguage.Text} \n{InsertColorTag($"{monthLeftUntilPayment} m.", goodGainColor)} {GetIconSpriteText(0, "Clock")}");
                }
                else
                {
                    sb.Append($"\n<size=130%>{InsertColorTag($"{billsPaymentMonthDangerousLanguage.Text}", badGainColor)} {GetIconSpriteText(0, "Cross")}</size>");
                }
            }

            int negativeSaturation = playerData.Food.NegativeSaturation;
            int saturationLimit = FoodData.NEGATIVE_SATURATION_LIMIT;
            string saturationText;
            string saturationColor;
            int saturationSize = 100;
            switch (negativeSaturation)
            {
                case int i when i < saturationLimit * 0.3f:
                    saturationText = saturationGoodLanguage.Text;
                    saturationColor = goodGainColor;
                    break;
                case int i when i < saturationLimit * 0.5f:
                    saturationText = saturationNormalLanguage.Text;
                    saturationColor = normalGainColor;
                    break;
                case int i when i < saturationLimit * 0.7f:
                    saturationSize = 110;
                    saturationText = saturationBadLanguage.Text;
                    saturationColor = badGainColor;
                    break;
                default:
                    saturationSize = 130;
                    saturationText = saturationDangerousLanguage.Text;
                    saturationColor = badGainColor;
                    break;
            }
            sb.Append($"\n\n{saturationText} \n<size={saturationSize}%>{InsertColorTag($"-{negativeSaturation}% {saturationLanguage.Text}", saturationColor)} {GetIconSpriteText(0, "Info")}</size>");

            MonthRandomActions randomActions = new();
            if (randomActions.TryGetAction(out var action))
            {
                sb.Append($"\n\n<align=center><size=100%>{action.Description.Text}</size></align><br>");
                int playerMoney = GameData.Data.PlayerData.Wallet.Value;
                int moneyChange = action.MoneyChange;
                int ratingChange = action.RatingChange;
                int moodChange = action.MoodChange;
                if (moneyChange > 0) GameData.Data.PlayerData.Wallet.TryIncreaseValue(moneyChange);
                else GameData.Data.PlayerData.Wallet.TryDecreaseValue(Mathf.Min(-moneyChange, playerMoney));

                if (ratingChange > 0) GameData.Data.CompanyData.Rating.TryIncreaseValue(ratingChange);
                else GameData.Data.CompanyData.Rating.TryDecreaseValue(-ratingChange);

                if (moodChange > 0) GameData.Data.PlayerData.Mood.TryIncreaseValue(moodChange);
                else GameData.Data.PlayerData.Mood.TryDecreaseValue(-moodChange);

                if (moneyChange != 0) AddTextBasedOnValue(sb, moneyChange, "Money", "", "", "", goodGainColor, badGainColor);
                if (ratingChange != 0) AddTextBasedOnValue(sb, ratingChange, "Rating", "", "", "", goodGainColor, badGainColor);
                if (moodChange != 0) AddTextBasedOnValue(sb, moodChange, "Mood", "", "", "", goodGainColor, badGainColor);
            }

            infoText.text = sb.ToString();
        }
        private static string GetIconSpriteText(int palette, string iconName) => $"<sprite=\"Palette-Icons-{palette}\" name=\"{iconName}\">";
        private void AddLineBasedOnValue(StringBuilder sb, int value, string styleName, string increaseLanguage, string decreaseLanguage, string zeroLanguage, string increaseGainColor, string decreaseGainColor)
        {
            if (value > 0)
            {
                string d = increaseLanguage.Length > 0 ? "\n" : ""; 
                sb.Append($"\n{increaseLanguage}{d}{InsertColorTag($"+<style={styleName}>{value}</style>", increaseGainColor)}<br>");
                return;
            }
            if (value < 0)
            {
                string d = decreaseLanguage.Length > 0 ? "\n" : ""; 
                sb.Append($"\n{decreaseLanguage}{d}{InsertColorTag($"-<style={styleName}>{Mathf.Abs(value)}</style>", decreaseGainColor)}<br>");
                return;
            }
            //== 0
            if (zeroLanguage.Length > 0)
            {
                sb.Append($"\n{zeroLanguage}<br>");
            }
        }
        /// <summary>
        /// Without new line
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="styleName"></param>
        /// <param name="increaseLanguage"></param>
        /// <param name="decreaseLanguage"></param>
        /// <param name="zeroLanguage"></param>
        /// <param name="increaseGainColor"></param>
        /// <param name="decreaseGainColor"></param>
        private void AddTextBasedOnValue(StringBuilder sb, int value, string styleName, string increaseLanguage, string decreaseLanguage, string zeroLanguage, string increaseGainColor, string decreaseGainColor)
        {
            if (value > 0)
            {
                string d = increaseLanguage.Length > 0 ? "\n" : "";
                sb.Append($"{increaseLanguage}{d}{InsertColorTag($"+<style={styleName}>{value}</style>", increaseGainColor)}<br>");
                return;
            }
            if (value < 0)
            {
                string d = decreaseLanguage.Length > 0 ? "\n" : "";
                sb.Append($"{decreaseLanguage}{d}{InsertColorTag($"-<style={styleName}>{Mathf.Abs(value)}</style>", decreaseGainColor)}<br>");
                return;
            }
            //== 0
            if (zeroLanguage.Length > 0)
            {
                sb.Append($"{zeroLanguage}<br>");
            }
        }
        private void UpdateBackgroundUI(MonthData.StatisticData gainStatistic)
        {
            int increasedValuesCount = gainStatistic.GetIncreasedValuesCount();
            switch (increasedValuesCount)
            {
                case int i when i >= 2: SetGoodBackground(); break;
                case int i when i == 1: SetNeutralBackground(); break;
                default: SetBadBackground(); break;
            }
        }
        private string InsertColorTag(string text, string color) => $"<color=#{color}>{text}</color>";
        private void SetGoodBackground() => background.color = goodBackgroundColor;
        private void SetBadBackground() => background.color = badBackgroundColor;
        private void SetNeutralBackground() => background.color = neutralBackgroundColor;

        #endregion methods
    }
}