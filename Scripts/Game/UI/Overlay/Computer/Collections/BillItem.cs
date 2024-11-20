using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Collections
{
    public class BillItem : ContextActionsItem<BillData>
    {
        #region fields & properties
        [SerializeField] private CustomButton buttonPay;
        [SerializeField] private TextMeshProUGUI billIdText;
        [SerializeField] private TextMeshProUGUI timeLeftText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI[] paymentAmountTexts;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            GameData.Data.PlayerData.Wallet.OnValueChanged += UpdateButtonUI;
            buttonPay.OnClicked += Pay;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            GameData.Data.PlayerData.Wallet.OnValueChanged -= UpdateButtonUI;
            buttonPay.OnClicked -= Pay;
        }
        private void Pay()
        {
            if (GameData.Data.PlayerData.BillsData.TryPayBill(Context)) return;
            new InfoRequest(null, LanguageInfo.GetTextByType(TextType.Game, 38), LanguageInfo.GetTextByType(TextType.Game, 37)).Send();
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            UpdateButtonUI();
            billIdText.text = $"{GameData.Data.PlayerData.BillsData.GetBillIndex(Context)}";
            timeLeftText.text = $"{Context.MonthUntilPayment} m.";
            int count = paymentAmountTexts.Length;
            string paymentAmount = $"${Context.PaymentAmount}";
            descriptionText.text = $"{Context.Description}";
            for (int i = 0; i < count; ++i)
            {
                paymentAmountTexts[i].text = paymentAmount;
            }

        }
        private bool CanPay() => GameData.Data.PlayerData.Wallet.CanDecreaseValue(Context.PaymentAmount);
        private void UpdateButtonUI(int _1, int _2) => UpdateButtonUI();
        private void UpdateButtonUI()
        {
            GameObject buttonObj = buttonPay.gameObject;
            bool active = CanPay();
            if (buttonObj.activeSelf != active)
                buttonObj.SetActive(active);
        }
        #endregion methods
    }
}