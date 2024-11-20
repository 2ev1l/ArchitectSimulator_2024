using System.Collections;
using System.Collections.Generic;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Events;

namespace Tests.PlayMode
{
    public partial class WorldDataTests
    {
        public class BillsTests
        {
            #region fields & properties
            private static readonly int defaultBillsPerMonth = 1;
            #endregion fields & properties

            #region methods
            [Test]
            public void HouseDataManualPaymentsPositiveTest() => DoManualPaymentsPositiveTest(HouseDataAction, 0);
            [Test]
            public void HouseDataManualPaymentsNegativeTest() => DoManualPaymentsNegativeTest(HouseDataAction, 0);
            [Test]
            public void HouseDataMonthPaymentsPositiveTest() => DoMonthPaymentsPositiveTest(HouseDataAction, 0);
            [Test]
            public void HouseDataMonthPaymentsNegativeTest() => DoMonthPaymentsNegativeTest(HouseDataAction, 0);
            private System.Action<GameData> HouseDataAction => (GameData data) =>
            {
                data.PlayerData.HouseData.TryReplaceInfo(0);
            };

            [Test]
            public void DivisionsDataManualPaymentsPositiveTest() => DoManualPaymentsPositiveTest(DivisionsAction, DivisionsBillsPerMonth);
            [Test]
            public void DivisionsDataManualPaymentsNegativeTest() => DoManualPaymentsNegativeTest(DivisionsAction, DivisionsBillsPerMonth);
            [Test]
            public void DivisionsDataMonthPaymentsPositiveTest() => DoMonthPaymentsPositiveTest(DivisionsAction, DivisionsBillsPerMonth);
            [Test]
            public void DivisionsDataMonthPaymentsNegativeTest() => DoMonthPaymentsNegativeTest(DivisionsAction, DivisionsBillsPerMonth);
            private System.Action<GameData> DivisionsAction => (GameData data) =>
            {
                OfficeAction.Invoke(data);
                data.CompanyData.OfficeData.TryHireBuilder(new(0));
                data.CompanyData.OfficeData.TryHireBuilder(new(0));
                BuilderData builderOn = (BuilderData)data.CompanyData.OfficeData.Divisions.Builders.Employees[0];
                BuilderData builderOff = (BuilderData)data.CompanyData.OfficeData.Divisions.Builders.Employees[1];
                builderOn.SetBusy();
                builderOff.SetFree();
            };
            private static readonly int DivisionsBillsPerMonth = 2; //office + builder on

            [Test]
            public void OfficeDataManualPaymentsPositiveTest() => DoManualPaymentsPositiveTest(OfficeAction);
            [Test]
            public void OfficeDataManualPaymentsNegativeTest() => DoManualPaymentsNegativeTest(OfficeAction);
            [Test]
            public void OfficeDataMonthPaymentsPositiveTest() => DoMonthPaymentsPositiveTest(OfficeAction);
            [Test]
            public void OfficeDataMonthPaymentsNegativeTest() => DoMonthPaymentsNegativeTest(OfficeAction);
            private System.Action<GameData> OfficeAction => (GameData data) => data.CompanyData.OfficeData.TryReplaceInfo(0);

            [Test]
            public void WarehouseDataManualPaymentsPositiveTest() => DoManualPaymentsPositiveTest(WarehouseAction);
            [Test]
            public void WarehouseDataManualPaymentsNegativeTest() => DoManualPaymentsNegativeTest(WarehouseAction);
            [Test]
            public void WarehouseDataMonthPaymentsPositiveTest() => DoMonthPaymentsPositiveTest(WarehouseAction);
            [Test]
            public void WarehouseDataMonthPaymentsNegativeTest() => DoMonthPaymentsNegativeTest(WarehouseAction);
            private System.Action<GameData> WarehouseAction => (GameData data) => data.CompanyData.WarehouseData.TryReplaceInfo(0);

            private void DoManualPaymentsPositiveTest(System.Action<GameData> allowBillAction, int billsPerMonth = 1)
            {
                PreapreAnyBillTest(allowBillAction);
                billsPerMonth += defaultBillsPerMonth;
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                BillsData bills = GameData.Data.PlayerData.BillsData;
                MonthData month = GameData.Data.PlayerData.MonthData;

                int walletValue = playerWallet.Value;
                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth, bills.Bills.Count);

                int paymentAmount = 0;
                for (int i = 0; i < billsPerMonth; ++i)
                {
                    paymentAmount += bills.Bills[0].PaymentAmount;
                    Assert.IsTrue(bills.TryPayBill(bills.Bills[0]));
                }
                Assert.AreEqual(0, bills.Bills.Count);
                Assert.AreEqual(walletValue - paymentAmount, playerWallet.Value);
                Assert.IsFalse(bills.HasUnpayedBills);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth, bills.Bills.Count);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth * 2, bills.Bills.Count);

                bills.TryPayBills();
                Assert.IsFalse(bills.HasUnpayedBills);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth, bills.Bills.Count);

                bills.TryPayBills();
                Assert.IsFalse(bills.HasUnpayedBills);
            }
            private void DoManualPaymentsNegativeTest(System.Action<GameData> allowBillAction, int billsPerMonth = 1)
            {
                PreapreAnyBillTest(allowBillAction);
                billsPerMonth += defaultBillsPerMonth;
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                BillsData bills = GameData.Data.PlayerData.BillsData;
                MonthData month = GameData.Data.PlayerData.MonthData;
                playerWallet.TryDecreaseValue(playerWallet.Value);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth, bills.Bills.Count);
                for (int i = 0; i < billsPerMonth; ++i)
                {
                    Assert.IsFalse(bills.TryPayBill(bills.Bills[0]));
                }
                Assert.AreEqual(billsPerMonth, bills.Bills.Count);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth * 2, bills.Bills.Count);

                bills.TryPayBills();
                Assert.IsTrue(bills.HasUnpayedBills);
            }
            private void DoMonthPaymentsPositiveTest(System.Action<GameData> allowBillAction, int billsPerMonth = 1)
            {
                PreapreAnyBillTest(allowBillAction);
                billsPerMonth += defaultBillsPerMonth;
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                MonthData month = GameData.Data.PlayerData.MonthData;
                BillsData bills = GameData.Data.PlayerData.BillsData;

                int walletValue = playerWallet.Value;
                int paymentTime = BillData.PAYMENT_TIME;
                for (int i = 1; i <= paymentTime; ++i)
                {
                    month.StartNextMonth();
                    Assert.AreEqual(billsPerMonth * i, bills.Bills.Count);
                    Assert.AreEqual(paymentTime - i + 1, bills.GetMinimalMonthUntilNextBill());
                    Assert.AreEqual(walletValue, playerWallet.Value);
                }

                int billPaymentAmount = 0;
                for (int i = 0; i < billsPerMonth; ++i)
                {
                    billPaymentAmount += bills.Bills[i].PaymentAmount;
                }

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth * paymentTime, bills.Bills.Count);
                Assert.AreEqual(1, bills.GetMinimalMonthUntilNextBill());
                Assert.AreEqual(walletValue - billPaymentAmount, playerWallet.Value);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth * paymentTime, bills.Bills.Count);
                Assert.AreEqual(1, bills.GetMinimalMonthUntilNextBill());
                Assert.AreEqual(walletValue - billPaymentAmount * 2, playerWallet.Value);
            }
            private void DoMonthPaymentsNegativeTest(System.Action<GameData> allowBillAction, int billsPerMonth = 1)
            {
                PreapreAnyBillTest(allowBillAction);
                billsPerMonth += defaultBillsPerMonth;
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                MonthData month = GameData.Data.PlayerData.MonthData;
                BillsData bills = GameData.Data.PlayerData.BillsData;
                playerWallet.TryDecreaseValue(playerWallet.Value);

                int walletValue = playerWallet.Value; //0
                int paymentTime = BillData.PAYMENT_TIME;
                for (int i = 1; i <= paymentTime; ++i)
                {
                    month.StartNextMonth();
                    Assert.AreEqual(billsPerMonth * i, bills.Bills.Count);
                    Assert.AreEqual(paymentTime - i + 1, bills.GetMinimalMonthUntilNextBill());
                    Assert.AreEqual(walletValue, playerWallet.Value);
                }

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth * (paymentTime + 1), bills.Bills.Count);
                Assert.AreEqual(0, bills.GetMinimalMonthUntilNextBill());
                Assert.AreEqual(walletValue, playerWallet.Value);
                Assert.IsTrue(bills.HasUnpayedBills);

                month.StartNextMonth();
                Assert.AreEqual(billsPerMonth * (paymentTime + 2), bills.Bills.Count);
                Assert.AreEqual(0, bills.GetMinimalMonthUntilNextBill());
                Assert.AreEqual(walletValue, playerWallet.Value);
                Assert.IsTrue(bills.HasUnpayedBills);
            }
            private void PreapreAnyBillTest(System.Action<GameData> allowBillAction)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                BillsData bills = GameData.Data.PlayerData.BillsData;
                Assert.AreEqual(0, bills.Bills.Count);
                playerWallet.TryIncreaseValue(100000);
                allowBillAction?.Invoke(GameData.Data);
                Assert.AreEqual(0, bills.Bills.Count);
            }
            #endregion methods
        }
    }
}