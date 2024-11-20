using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.DataBase;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Core;
using Universal.Events;

namespace Tests.PlayMode
{
    public partial class WorldDataTests
    {
        public class MonthTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void IncreaseMonthFoodTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                MonthData monthData = GameData.Data.PlayerData.MonthData;
                FoodData food = GameData.Data.PlayerData.Food;
                food.TryIncreaseSaturation(1);
                monthData.StartNextMonth();
                Assert.IsTrue(food.CanIncreaseSaturation);
                Assert.IsTrue(food.TryIncreaseSaturation(1));
                Assert.AreEqual(2, food.TotalSaturation);
            }
            [Test]
            public void IncreaseMonthLandPlotsOffersTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
                plots.TryAdd(0);
                LandPlotData plot = plots.Plots[0];
                MonthData monthData = GameData.Data.PlayerData.MonthData;
                for (int i = 0; i < 10; ++i)
                {
                    monthData.StartNextMonth();
                    Assert.AreEqual(0, plots.Offers.Count);
                }
                plots.TryStartSelling(0, plot.MaxSellPrice / 2);
                for (int i = 0; i < 10; ++i)
                {
                    monthData.StartNextMonth();
                    Assert.IsTrue(plots.Offers.Count == 0 || plots.Offers.Count == 1 || plots.Offers.Count == 2);
                }
            }

            [Test]
            public void IncreaseMonthBillsDataTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                GameData.Data.PlayerData.Wallet.TryIncreaseValue(100000);
                BillsData bills = GameData.Data.PlayerData.BillsData;
                Assert.AreEqual(0, bills.Bills.Count);
                GameData.Data.CompanyData.OfficeData.TryReplaceInfo(0);
                MonthData monthData = GameData.Data.PlayerData.MonthData;
                Assert.IsTrue(GameData.Data.CompanyData.OfficeData.CanAddBill);
                Assert.IsFalse(GameData.Data.CompanyData.OfficeData.Divisions.CanAddBill);
                monthData.StartNextMonth();
                Assert.AreEqual(2, bills.Bills.Count);
            }

            [Test]
            public void IncreaseMonthConstructionTasksGenerateTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                var tasks = GameData.Data.CompanyData.ConstructionTasks;
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                MonthData month = GameData.Data.PlayerData.MonthData;
                month.StartNextMonth();
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                int safety = 100;
                int counter = 0;
                while (tasks.StartedTasks.Count == 0)
                {
                    counter++;
                    month.StartNextMonth();
                    if (counter > safety)
                        throw new InvalidProgramException();
                }
                Assert.IsTrue(tasks.StartedTasks.Count > 0);
            }

            [Test]
            public void IncreaseMonthConstructionTasksTest() => DoIncreaseMonthTaskTest(x => x.CompanyData.ConstructionTasks, x => x.ConstructionTaskInfo.Data.Select(x => x.Data), false);
            [Test]
            public void IncreaseMonthPlayerTasksTest() => DoIncreaseMonthTaskTest(x => x.PlayerData.Tasks, x => x.PlayerTaskInfo.Data.Select(x => x.Data));

            [Test]
            public void IncreaseMonthEnvironmentTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                EnvironmentData environment = GameData.Data.EnvironmentData;
                environment.LastTransportUsed = TransportType.Bus;
                GameData.Data.PlayerData.MonthData.StartNextMonth();
                Assert.AreEqual(environment.LastTransportUsed, TransportType.Unknown);
            }
            [Test]
            public void IncreaseMonthLandPlotsShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.LandPlotShop);
            [Test]
            public void IncreaseMonthOfficeShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.OfficeShop);
            [Test]
            public void IncreaseMonthWarehouseShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.WarehouseShop);
            [Test]
            public void IncreaseMonthBuildersRecruitShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.BuildersRecruit);
            [Test]
            public void IncreaseMonthHRRecruitShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.HRRecruit);
            [Test]
            public void IncreaseMonthPRRecruitShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.PRRecruit);
            [Test]
            public void IncreaseMonthDesignEngineerRecruitRecruitShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.DesignEngineerRecruit);
            [Test]
            public void IncreaseMonthConstructionResourceShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.ConstructionResourceShop);
            [Test]
            public void IncreaseMonthHouseShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.HouseShop);
            [Test]
            public void IncreaseMonthFoodShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.FoodShop);
            [Test]
            public void IncreaseMonthVehicleShopDataTest() => DoIncreaseMonthShopCartDataTest(x => x.VehicleShop);

            [Test]
            public void ChangeMonthStatisticTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                PlayerData player = GameData.Data.PlayerData;
                MonthData month = player.MonthData;
                Assert.AreEqual(1, month.CurrentMonth);
                month.StartNextMonth();
                Assert.AreEqual(2, month.CurrentMonth);
                Assert.AreEqual(player.Wallet.Value, month.GainStatistic.Money);
                Assert.AreEqual(0, month.GainStatistic.Saturation);
                Assert.AreEqual(0, month.GainStatistic.Time);
                Assert.AreEqual(0, month.GainStatistic.Mood);
                Assert.AreEqual(0, month.GainStatistic.CompletedTasks);

                int moneyIncrease = 99;
                int timeDecrease = 18;
                int moodDecrease = 21;
                int saturationIncrease = 5;
                player.Wallet.TryIncreaseValue(moneyIncrease);
                month.FreeTime.TryDecreaseValue(timeDecrease);
                player.Mood.TryDecreaseValue(moodDecrease);
                player.Food.TryIncreaseSaturation(saturationIncrease);
                Assert.AreEqual(5, player.Food.TotalSaturation);
                player.Tasks.TryStartTask(0);
                player.Tasks.TryCompleteTask(0);
                int oldWallet = player.Wallet.Value;
                month.StartNextMonth();

                Assert.AreEqual(3, month.CurrentMonth);
                Assert.AreEqual(moneyIncrease, month.GainStatistic.Money);
                Assert.AreEqual(saturationIncrease, month.GainStatistic.Saturation);
                Assert.IsTrue(month.GainStatistic.Time < 0); //mood and free time are interdependent so it's hard to calculate test statistic for them
                Assert.IsTrue(month.GainStatistic.Mood < 0);
                Assert.AreEqual(1, month.GainStatistic.CompletedTasks);

                Assert.AreEqual(oldWallet, month.StartStatistic.Money);
                Assert.AreEqual(1, month.StartStatistic.CompletedTasks);

                int moneyDecrease = 50;
                timeDecrease = 46;
                moodDecrease = 40;
                saturationIncrease = 10;
                player.Wallet.TryDecreaseValue(moneyDecrease);
                month.FreeTime.TryDecreaseValue(timeDecrease);
                player.Mood.TryDecreaseValue(moodDecrease);
                player.Food.TryIncreaseSaturation(saturationIncrease);
                oldWallet = player.Wallet.Value;

                month.StartNextMonth();
                Assert.AreEqual(4, month.CurrentMonth);
                Assert.AreEqual(-moneyDecrease, month.GainStatistic.Money);
                Assert.AreEqual(saturationIncrease, month.GainStatistic.Saturation);
                Assert.IsTrue(month.GainStatistic.Time < 0);
                Assert.IsTrue(month.GainStatistic.Mood < 0);
                Assert.AreEqual(0, month.GainStatistic.CompletedTasks);

                Assert.AreEqual(oldWallet, month.StartStatistic.Money);
                Assert.AreEqual(1, month.StartStatistic.CompletedTasks);

                month.StartNextMonth();
                Assert.AreEqual(5, month.CurrentMonth);
                Assert.IsTrue(month.GainStatistic.Money < 0);
                Assert.AreEqual(0, month.GainStatistic.Saturation);
                Assert.IsTrue(month.GainStatistic.Time > 0);
                Assert.IsTrue(month.GainStatistic.Mood > 0);
                Assert.AreEqual(0, month.GainStatistic.CompletedTasks);
            }
            private void DoIncreaseMonthTaskTest<Task, Info>(Func<GameData, TasksData<Task, Info>> getTasks, Func<DB, IEnumerable<Info>> getDBSet, bool taskMustBeExpired = true)
                where Task : TaskData<Info>
                where Info : TaskInfo
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                var tasks = getTasks.Invoke(GameData.Data);
                var dbSet = getDBSet.Invoke(DB.Instance);
                Info taskInfo = dbSet.Where(x => x.MonthDuration > 0).First();
                int taskDuration = taskInfo.MonthDuration;
                tasks.TryStartTask(taskInfo.Id);
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                tasks.IsTaskStarted(taskInfo.Id, out Task startedTask);
                MonthData month = GameData.Data.PlayerData.MonthData;
                int safety = taskDuration + 1;
                int counter = 0;
                while (!startedTask.IsLastMonthBeforeExpiration())
                {
                    counter++;
                    month.StartNextMonth();
                    if (counter > safety)
                        throw new InvalidProgramException();
                }
                Assert.IsTrue(tasks.IsTaskStarted(taskInfo.Id, out _));
                Assert.IsFalse(tasks.IsTaskExpired(taskInfo.Id, out _));
                month.StartNextMonth();
                Assert.IsFalse(tasks.IsTaskStarted(taskInfo.Id, out _));
                if (taskMustBeExpired)
                    Assert.IsTrue(tasks.IsTaskExpired(taskInfo.Id, out _));

                GameData.SetData(new());
                tasks = getTasks.Invoke(GameData.Data);
                dbSet = getDBSet.Invoke(DB.Instance);
                var taskDuration2 = dbSet.Where(x => x.MonthDuration == 2).First();
                if (taskDuration2 != null)
                {
                    Assert.IsTrue(tasks.TryStartTask(taskDuration2.Id));
                    Assert.AreEqual(0, tasks.ExpiredTasks.Count);
                    GameData.Data.PlayerData.MonthData.StartNextMonth();
                    Assert.AreEqual(0, tasks.ExpiredTasks.Count);
                    GameData.Data.PlayerData.MonthData.StartNextMonth();
                    if (taskMustBeExpired)
                        Assert.AreEqual(1, tasks.ExpiredTasks.Count);
                }
            }
            private void DoIncreaseMonthShopDataTest<T>(Func<BrowserData, ShopData<T>> getShop) where T : ShopItemData, ICloneable<T>
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                var shop = getShop.Invoke(GameData.Data.BrowserData);
                Assert.IsTrue(shop.Items.Count == 0);
                GameData.Data.PlayerData.MonthData.StartNextMonth();
                Assert.IsTrue(shop.Items.Count > 0);
                var oldItems = shop.Items;
                GameData.Data.PlayerData.MonthData.StartNextMonth();
                for (int i = 0; i < shop.Items.Count; ++i)
                {
                    Assert.AreNotSame(oldItems[i], shop.Items[i]);
                }
            }
            private void DoIncreaseMonthShopCartDataTest<T>(Func<BrowserData, ShopCartData<T>> getShop) where T : ShopItemData, ICloneable<T>
            {
                DoIncreaseMonthShopDataTest(getShop);
                var shop = getShop.Invoke(GameData.Data.BrowserData);
                var cart = shop.Cart;
                Assert.AreEqual(0, cart.Items.Count);
                shop.Items.Exists(x => x.Id == 0, out var item);
                cart.Add(item, 1);
                Assert.AreEqual(1, cart.Items.Count);
                GameData.Data.PlayerData.MonthData.StartNextMonth();
                Assert.AreEqual(0, cart.Items.Count);
            }
            #endregion methods
        }
    }
}