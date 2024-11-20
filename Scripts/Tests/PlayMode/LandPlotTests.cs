using System;
using System.Collections;
using System.Collections.Generic;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Events;

namespace Tests.PlayMode
{
    partial class WorldDataTests
    {

        public class LandPlotTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void OfferRemoveTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                plots.TryAdd(0);
                Assert.IsTrue(plots.TryStartSelling(0, 1));
                if (!TryGenerateOffer(plots)) return;
                Assert.IsTrue(plots.TryEndSelling(0));
                Assert.AreEqual(0, plots.Offers.Count);
                Assert.IsTrue(plots.TryStartSelling(0, 1));

                if (!TryGenerateOffer(plots)) return;
                Assert.IsTrue(plots.TrySellPlot(0, 1));
                Assert.AreEqual(0, plots.Offers.Count);
            }
            [Test]
            public void OfferConfirmPositiveTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                plots.TryAdd(0);
                plots.TryStartSelling(0, 1);
                if (!TryGenerateOffer(plots)) return;
                int walletValue = playerWallet.Value;
                plots.ConfirmOffer(plots.Offers[0]);
                Assert.AreEqual(walletValue + 1, playerWallet.Value);
            }
            [Test]
            public void OfferConfirmNegativeTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                plots.TryAdd(0);
                Assert.IsTrue(plots.TryStartSelling(0, 1));
                if (!TryGenerateOffer(plots)) return;
                plots.ConfirmOffer(null);
                Assert.AreEqual(1, plots.Offers.Count);
            }
            //somtimes may throw error because of random
            private bool TryGenerateOffer(LandPlotsData plots)
            {
                MonthData month = GameData.Data.PlayerData.MonthData;
                int safety = 100;
                for (int i = 0; i <= safety; ++i)
                {
                    month.StartNextMonth();
                    if (plots.Offers.Count == 1) break;
                    if (i == safety)
                    {
                        Debug.LogError("Can't generate offer");
                        throw new System.InvalidProgramException();
                    }
                }
                Assert.AreEqual(1, plots.Offers.Count);
                return true;
            }
            [Test]
            public void SellingPositiveTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                int walletValue = playerWallet.Value;
                plots.TryAdd(0);
                Assert.IsTrue(plots.TryStartSelling(0, 2));
                Assert.AreEqual(1, plots.Plots.Count);
                Assert.AreEqual(1, plots.SellingPlots.Count);
                Assert.IsTrue(plots.TryEndSelling(0));
                Assert.AreEqual(1, plots.Plots.Count);
                Assert.AreEqual(0, plots.SellingPlots.Count);
                plots.TryAdd(1);
                Assert.IsTrue(plots.TryStartSelling(1, 5));
                Assert.IsTrue(plots.TrySellPlot(1, 6));
                Assert.AreEqual(2, plots.Plots.Count);
                Assert.AreEqual(1, plots.SoldPlots.Count);
                Assert.AreEqual(walletValue + 6, playerWallet.Value);
            }
            [Test]
            public void SellingNegativeTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                int walletValue = playerWallet.Value;
                plots.TryAdd(0);
                Assert.IsFalse(plots.TryStartSelling(0, -1));
                Assert.IsFalse(plots.TryStartSelling(0, 0));
                Assert.IsFalse(plots.TryStartSelling(-1, 2));
                Assert.IsFalse(plots.TryStartSelling(1, 2));
                Assert.IsTrue(plots.TryStartSelling(0, 2));
                Assert.IsFalse(plots.TryStartSelling(0, 2));

                Assert.IsFalse(plots.TryEndSelling(-1));
                Assert.IsFalse(plots.TryEndSelling(1));
                Assert.IsTrue(plots.TryEndSelling(0));
                Assert.IsFalse(plots.TryEndSelling(0));

                Assert.IsTrue(plots.TryStartSelling(0, 2));
                Assert.IsFalse(plots.TrySellPlot(0, -1));
                Assert.IsFalse(plots.TrySellPlot(0, 0));
                Assert.IsFalse(plots.TrySellPlot(-1, 0));
                Assert.IsTrue(plots.TrySellPlot(0, 1));
                Assert.IsFalse(plots.TrySellPlot(0, 1));
                Assert.AreEqual(walletValue + 1, playerWallet.Value);
            }
            [Test]
            public void AddPositiveTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                Assert.AreEqual(0, plots.Plots.Count);
                Assert.IsTrue(plots.TryAdd(0));
                Assert.AreEqual(1, plots.Plots.Count);
                Assert.AreEqual(0, plots.Plots[0].Id);
                Assert.IsTrue(plots.TryAdd(1));
                Assert.AreEqual(2, plots.Plots.Count);
                Assert.AreEqual(1, plots.Plots[1].Id);
                Assert.IsTrue(plots.Exists(0, out _));
                Assert.IsTrue(plots.Exists(1, out _));
            }
            [Test]
            public void AddNegativeTest()
            {
                PrepareAnyTest(out LandPlotsData plots);
                Assert.IsFalse(plots.TryAdd(-1));
                Assert.IsFalse(plots.TryAdd(-24));
                Assert.IsTrue(plots.TryAdd(1));
                Assert.IsFalse(plots.TryAdd(1));
                Assert.AreEqual(1, plots.Plots.Count);
            }

            private void PrepareAnyTest(out LandPlotsData plots)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                plots = GameData.Data.CompanyData.LandPlotsData;
            }
            #endregion methods
        }
    }
}