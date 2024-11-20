using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BillsData : IMonthUpdatable
    {
        #region fields & properties
        public UnityAction OnBillPayed;
        public IReadOnlyList<BillData> Bills => bills;
        [SerializeField] private List<BillData> bills = new();
        public bool HasUnpayedBills => hasUnpayedBills;
        [SerializeField] private bool hasUnpayedBills = false;
        #endregion fields & properties

        #region methods
        public int GetBillIndex(BillData bill) => bills.IndexOf(bill);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>0..3</returns>
        public int GetMinimalMonthUntilNextBill()
        {
            int billsCount = bills.Count;
            int min = int.MaxValue;
            for (int i = 0; i < billsCount; ++i)
            {
                int newMin = bills[i].MonthUntilPayment;
                if (newMin < min)
                    min = newMin;
            }
            return min;
        }
        public int GetBillsSum()
        {
            int billsCount = bills.Count;
            int sum = 0;
            for (int i = 0; i < billsCount; ++i)
            {
                sum += bills[i].PaymentAmount;
            }
            return sum;
        }
        public void OnMonthUpdate(MonthData monthData)
        {
            PayMonthBills();
            AddBills();
        }
        internal void AddBills()
        {
            IReadOnlyList<IBillPayable> billPayables = GameData.Data.GetBillPayables();
            int billPayablesCount = billPayables.Count;
            for (int i = 0; i < billPayablesCount; ++i)
            {
                IBillPayable billPayable = billPayables[i];
                if (!billPayable.CanAddBill) continue;
                if (billPayable.BillPaymentAmount <= 0) continue;
                AddBill(billPayables[i]);
            }
        }
        private void AddBill(IBillPayable billPayable) => bills.Add(new(billPayable));
        private void PayMonthBills()
        {
            Wallet playerWallet = GameData.Data.PlayerData.Wallet;
            int i = bills.Count - 1;
            bool hasUnpayed = false;
            while (i >= 0)
            {
                BillData bill = bills[i];
                if (!bill.MustBePayed())
                {
                    --i;
                    continue;
                }
                if (!bill.TryPay(playerWallet))
                {
                    hasUnpayed = true;
                }
                else
                {
                    bills.RemoveAt(i);
                }
                --i;
            }
            hasUnpayedBills = hasUnpayed;
        }
        public void TryPayBills()
        {
            Wallet playerWallet = GameData.Data.PlayerData.Wallet;
            int i = bills.Count - 1;
            bool hasUnpayed = false;
            while (i >= 0)
            {
                BillData bill = bills[i];
                if (!bill.TryPay(playerWallet))
                {
                    hasUnpayed = true;
                }
                else
                {
                    bills.RemoveAt(i);
                    OnBillPayed?.Invoke();
                }
                --i;
            }
            hasUnpayedBills = hasUnpayed;
        }
        public bool TryPayBill(BillData bill)
        {
            if (bill.TryPay(GameData.Data.PlayerData.Wallet))
            {
                bills.Remove(bill);
                OnBillPayed?.Invoke();
                return true;
            }
            return false;
        }
        #endregion methods
    }
}