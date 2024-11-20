using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BillData
    {
        #region fields & properties
        public const int PAYMENT_TIME = 3;
        [SerializeField][Min(0)] private int paymentMonth;
        public int PaymentAmount => paymentAmount;
        [SerializeField][Min(1)] private int paymentAmount;
        public string Description => description;
        [SerializeField] private string description;
        private static int CurrentMonth => GameData.Data.PlayerData.MonthData.CurrentMonth;
        /// <summary>
        /// 0..<see cref="PAYMENT_TIME"/>
        /// </summary>
        public int MonthUntilPayment => Mathf.Max(paymentMonth - CurrentMonth, 0);
        #endregion fields & properties

        #region methods
        internal bool TryPay(Wallet wallet)
        {
            return wallet.TryDecreaseValue(paymentAmount);
        }
        internal bool MustBePayed() => CurrentMonth >= paymentMonth;

        /// <summary>
        /// Runtime only
        /// </summary>
        /// <param name="paymentAmount"></param>
        public BillData(IBillPayable billPayable)
        {
            this.paymentAmount = billPayable.BillPaymentAmount;
            this.paymentMonth = CurrentMonth + PAYMENT_TIME;
            this.description = billPayable.BillDescription;
        }
        public BillData(int currentMonth, int paymentAmount, string description)
        {
            this.paymentMonth = currentMonth + PAYMENT_TIME;
            this.paymentAmount = paymentAmount;
            this.description = description;
        }
        #endregion methods
    }
}