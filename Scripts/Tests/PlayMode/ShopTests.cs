using System;
using System.Collections;
using System.Collections.Generic;
using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Core;
using Universal.Events;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Tests.PlayMode
{
    public partial class WorldDataTests
    {

        public class ShopTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void VehicleCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.VehicleShop);
            [Test]
            public void VehicleCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.VehicleShop);
            [Test]
            public void VehicleCartPurchasePositiveTest()
            {
                PrepareCartWithItem(0, 1, x => x.VehicleShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.AreEqual(0, GameData.Data.PlayerData.VehicleData.Id);

                shop.Items.Exists(x => x.Id == 1, out var item1);
                shop.Cart.Add(item1, 1);
                Wallet wallet = GameData.Data.PlayerData.Wallet;
                int finalValue = wallet.Value - item1.FinalPrice + Mathf.RoundToInt(GameData.Data.PlayerData.VehicleData.BuyableInfo.Price * VehicleShopItemData.ReturnValue);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.AreEqual(1, GameData.Data.PlayerData.VehicleData.Id);
                Assert.AreEqual(finalValue, wallet.Value);
            }
            [Test]
            public void VehicleCartPurchaseNegativeTest()
            {
                PrepareCartWithItem(0, -1, x => x.VehicleShop, out var shop, out var item);
                Assert.AreEqual(0, shop.Cart.Items.Count);
                Assert.IsFalse(shop.Cart.CanPurchaseCart());
            }

            [Test]
            public void FoodCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.FoodShop);
            [Test]
            public void FoodCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.FoodShop);
            [Test]
            public void FoodCartPurchasePositiveTest()
            {
                int foodWithNegativeMood = DB.Instance.BuyableFoodInfo.Find(x => x.Data.Info.MoodChange < 0).Data.Id;
                PrepareCartWithItem(foodWithNegativeMood, 1, x => x.FoodShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                Assert.IsTrue(GameData.Data.PlayerData.Food.TotalSaturation == 0);
                Assert.IsTrue(GameData.Data.PlayerData.Mood.Value == 100);
                shop.Cart.PurchaseCart();
                Assert.IsTrue(GameData.Data.PlayerData.Food.TotalSaturation > 0);
                Assert.IsTrue(GameData.Data.PlayerData.Mood.Value < 100);
                Assert.IsFalse(shop.Cart.CanPurchaseCart());
            }
            [Test]
            public void FoodCartPurchaseNegativeTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                Assert.IsTrue(GameData.Data.PlayerData.Food.TryIncreaseSaturation(10));
                GameData.Data.PlayerData.Wallet.TryIncreaseValue(100000);
                var shop = GameData.Data.BrowserData.FoodShop;
                int id = 0;
                shop.GenerateNewData();
                shop.Items.Exists(x => x.Id == id, out var item);
                shop.Cart.Add(item, 1);
                Assert.IsFalse(GameData.Data.PlayerData.Food.CanIncreaseSaturation);
                Assert.IsFalse(shop.Cart.CanPurchaseCart());
            }

            [Test]
            public void HouseCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.HouseShop);
            [Test]
            public void HouseCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.HouseShop);
            [Test]
            public void HouseCartPurchasePositiveTest()
            {
                PrepareCartWithItem(1, 1, x => x.HouseShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.AreEqual(1, GameData.Data.PlayerData.HouseData.Id);

                shop.Items.Exists(x => x.Id == 0, out var item0);
                shop.Cart.Add(item0, 1);
                Wallet wallet = GameData.Data.PlayerData.Wallet;
                int finalValue = wallet.Value - item0.FinalPrice + Mathf.RoundToInt(GameData.Data.PlayerData.HouseData.RentableInfo.Price * HouseShopItemData.ReturnValue);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.AreEqual(0, GameData.Data.PlayerData.HouseData.Id);
                Assert.AreEqual(finalValue, wallet.Value);
            }
            [Test]
            public void HouseCartPurchaseNegativeTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                GameData.Data.PlayerData.Wallet.TryIncreaseValue(100000);
                var house = GameData.Data.PlayerData.HouseData;
                HouseInfoSO houseWithManyPeople = DB.Instance.HouseInfo.Find(x => x.Data.MaxPeople > 1);
                if (houseWithManyPeople == null)
                {
                    Debug.LogError("Can't do test because can't find house with people count > 1");
                    throw new System.InvalidProgramException();
                }
                Assert.IsTrue(house.TryReplaceInfo(houseWithManyPeople.Id));
                Assert.IsTrue(house.TryAddPeople());

                var shop = GameData.Data.BrowserData.HouseShop;
                int id = 0;
                shop.GenerateNewData();
                shop.Items.Exists(x => x.Id == id, out var item);
                shop.Cart.Add(item, 1);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsFalse(shop.Cart.CanPurchaseCart());
            }
            
            [Test]
            public void RecruitDesignEngineerCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.DesignEngineerRecruit);
            [Test]
            public void RecruitDesignEngineerCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.DesignEngineerRecruit);
            [Test]
            public void RecruitDesignEngineerCartPurchasePositiveTest() => DoRecruitEmployeeCartPurchasePositiveTest<RecruitDesignEngineerData, DesignEngineerData>(x => x.DesignEngineerRecruit);
            [Test]
            public void RecruitDesignEngineerCartPurchaseNegativeTest() => DoRecruitEmployeeCartPurchaseNegativeTest<RecruitDesignEngineerData, DesignEngineerData>(x => x.DesignEngineerRecruit);

            [Test]
            public void RecruitPRCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.PRRecruit);
            [Test]
            public void RecruitPRCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.PRRecruit);
            [Test]
            public void RecruitPRCartPurchasePositiveTest() => DoRecruitEmployeeCartPurchasePositiveTest<RecruitPRData, PRManagerData>(x => x.PRRecruit);
            [Test]
            public void RecruitPRCartPurchaseNegativeTest() => DoRecruitEmployeeCartPurchaseNegativeTest<RecruitPRData, PRManagerData>(x => x.PRRecruit);

            [Test]
            public void RecruitHRCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.HRRecruit);
            [Test]
            public void RecruitHRCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.HRRecruit);
            [Test]
            public void RecruitHRCartPurchasePositiveTest() => DoRecruitEmployeeCartPurchasePositiveTest<RecruitHRData, HRManagerData>(x => x.HRRecruit);
            [Test]
            public void RecruitHRCartPurchaseNegativeTest() => DoRecruitEmployeeCartPurchaseNegativeTest<RecruitHRData, HRManagerData>(x => x.HRRecruit);

            [Test]
            public void RecruitBuilderCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.BuildersRecruit);
            [Test]
            public void RecruitBuilderCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.BuildersRecruit);
            [Test]
            public void RecruitBuilderCartPurchasePositiveTest() => DoRecruitEmployeeCartPurchasePositiveTest<RecruitBuilderData, BuilderData>(x => x.BuildersRecruit);
            [Test]
            public void RecruitBuilderCartPurchaseNegativeTest() => DoRecruitEmployeeCartPurchaseNegativeTest<RecruitBuilderData, BuilderData>(x => x.BuildersRecruit);

            [Test]
            public void RentableOfficeCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.OfficeShop);
            [Test]
            public void RentableOfficeCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.OfficeShop);
            [Test]
            public void RentableOfficeCartPurchasePositiveTest() => DoRentablePremiseCartPurchasePositiveTest<RentableOfficeShopItemData, RentableOffice>(x => x.OfficeShop, x => x.OfficeData);
            [Test]
            public void RentableOfficeCartPurchaseNegativeTest()
            {
                AssetLoader.InitInstances();
                OfficeInfo minEmployeesOffice = DB.Instance.OfficeInfo[0].Data;
                OfficeInfo maxEmployeesOffice = DB.Instance.OfficeInfo.Find(x => x.Data.MaximumEmployees > minEmployeesOffice.MaximumEmployees).Data;
                if (maxEmployeesOffice == null)
                {
                    Debug.LogError($"Can't do test because can't find office with more employees");
                    throw new System.InvalidProgramException();
                }
                PrepareCartWithItem(minEmployeesOffice.Id, 1, x => x.OfficeShop, out var shop, out var item);
                OfficeData office = GameData.Data.CompanyData.OfficeData;
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                office.TryReplaceInfo(maxEmployeesOffice.Id);
                BuilderData builder = new(0);
                int maximumEmployees = maxEmployeesOffice.MaximumEmployees;
                for (int i = 0; i < maximumEmployees; ++i)
                {
                    office.TryHireBuilder(builder);
                }
                Assert.IsTrue(!shop.Cart.CanPurchaseCart());
            }

            [Test]
            public void RentableWarehouseCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.WarehouseShop);
            [Test]
            public void RentableWarehouseCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.WarehouseShop);
            [Test]
            public void RentableWarehouseCartPurchasePositiveTest() => DoRentablePremiseCartPurchasePositiveTest<RentableWarehouseShopItemData, RentableWarehouse>(x => x.WarehouseShop, x => x.WarehouseData);
            [Test]
            public void RentableWarehouseCartPurchaseNegativeTest()
            {
                AssetLoader.InitInstances();
                WarehouseInfo minSpaceWarehouse = DB.Instance.WarehouseInfo[0].Data;
                WarehouseInfo maxSpaceWarehouse = DB.Instance.WarehouseInfo.Find(x => x.Data.Space > minSpaceWarehouse.Space).Data;
                if (maxSpaceWarehouse == null)
                {
                    Debug.LogError($"Can't do test because can't find warehouse with more space");
                    throw new System.InvalidProgramException();
                }
                PrepareCartWithItem(0, 1, x => x.WarehouseShop, out var shop, out var item);

                WarehouseData warehouse = GameData.Data.CompanyData.WarehouseData;
                warehouse.TryReplaceInfo(maxSpaceWarehouse.Id);
                ConstructionResourceData res = new(0);
                float singleResVolume = res.GetTotalVolumeM3();
                int itemsToFullfillWarehouse = Mathf.FloorToInt((float)warehouse.FreeSpace / singleResVolume);
                res.Add(itemsToFullfillWarehouse - 1);
                warehouse.TryAddConstructionResource(res);

                Assert.IsTrue(warehouse.FreeSpace < singleResVolume);
                if (warehouse.OccupiedSpace < minSpaceWarehouse.Space)
                {
                    Debug.LogError("Can't do test because minSpaceWarehouse space is near to maxSpaceWarehouse");
                    throw new System.InvalidProgramException();
                }
                Assert.IsTrue(!shop.Cart.CanPurchaseCart());
            }

            [Test]
            public void RentableLandPlotCartAddRemoveItemPositiveTest() => DoCartSingleAddRemoveItemPositiveTest(x => x.LandPlotShop);
            [Test]
            public void RentableLandPlotCartAddRemoveItemNegativeTest() => DoCartSingleAddRemoveItemNegativeTest(x => x.LandPlotShop);
            [Test]
            public void RentableLandPlotCartPurchasePositiveTest()
            {
                PrepareCartWithItem(0, 1, x => x.LandPlotShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.IsTrue(GameData.Data.CompanyData.LandPlotsData.Exists(0, out _));
            }
            [Test]
            public void RentableLandPlotCartPurchaseNegativeTest()
            {
                PrepareCartWithItem(0, 1, x => x.LandPlotShop, out var shop, out var item);
                GameData.Data.CompanyData.LandPlotsData.TryAdd(0);
                Assert.IsFalse(shop.Cart.CanPurchaseCart());
            }


            [Test]
            public void ConstructionResourceCartAddRemoveItemPositiveTest() => DoCartAddRemoveItemPositiveTest(x => x.ConstructionResourceShop);
            [Test]
            public void ConstructionResourceCartAddRemoveItemNegativeTest() => DoCartAddRemoveItemNegativeTest(x => x.ConstructionResourceShop);
            [Test]
            public void ConstructionResourceCartPurchasePositiveTest()
            {
                PrepareCartWithItem(0, 1, x => x.ConstructionResourceShop, out var shop, out _);
                Assert.IsTrue(!shop.Cart.CanPurchaseCart());
                WarehouseData warehouse = GameData.Data.CompanyData.WarehouseData;
                warehouse.TryReplaceInfo(0);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.IsTrue(warehouse.ConstructionResources.Items.Count > 0);
            }
            [Test]
            public void ConstructionResourceCartPurchaseNegativeTest()
            {
                PrepareCartWithItem(0, -1, x => x.ConstructionResourceShop, out var shop, out _);
                WarehouseData warehouse = GameData.Data.CompanyData.WarehouseData;
                warehouse.TryReplaceInfo(0);
                Assert.IsTrue(!shop.Cart.CanPurchaseCart());
                Assert.IsTrue(warehouse.ConstructionResources.Items.Count == 0);
            }


            private void DoRecruitEmployeeCartPurchaseNegativeTest<ShopItem, Recruit>(Func<BrowserData, ShopCartData<ShopItem>> getShop)
                where ShopItem : RecruitEmployeeData<Recruit>, ICloneable<ShopItem>, ISingleShopItem
                where Recruit : EmployeeData
            {
                PrepareCartWithItem(0, -10, getShop, out var shop, out var recruit);
                Assert.AreEqual(0, shop.Cart.Items.Count);
                OfficeData office = GameData.Data.CompanyData.OfficeData;
                OfficeInfo officeInfoWithManyEmployees = DB.Instance.OfficeInfo.Find(x => x.Data.MaximumEmployees > 1).Data;
                if (officeInfoWithManyEmployees == null)
                {
                    Debug.Log("Can't do test because office info with maximum employees > 1 not exist");
                    throw new System.InvalidProgramException();
                }
                office.TryReplaceInfo(officeInfoWithManyEmployees.Id);
                int maxEmployees = officeInfoWithManyEmployees.MaximumEmployees;
                Assert.AreEqual(0, office.Divisions.GetEmployeesCount());

                Assert.IsTrue(!shop.Cart.CanPurchaseCart());
                shop.Cart.Add(recruit, 100);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.AreEqual(1, office.Divisions.GetEmployeesCount());
            }
            private void DoRecruitEmployeeCartPurchasePositiveTest<ShopItem, Recruit>(Func<BrowserData, ShopCartData<ShopItem>> getShop)
                where ShopItem : RecruitEmployeeData<Recruit>, ICloneable<ShopItem>, ISingleShopItem
                where Recruit : EmployeeData
            {
                PrepareCartWithItem(0, 1, getShop, out var shop, out var recruit);
                Assert.AreEqual(1, shop.Items.Count);
                OfficeData office = GameData.Data.CompanyData.OfficeData;
                OfficeInfo officeInfoWithManyEmployees = DB.Instance.OfficeInfo.Find(x => x.Data.MaximumEmployees > 1).Data;
                if (officeInfoWithManyEmployees == null)
                {
                    Debug.LogError("Can't do test because office info with maximum employees > 1 not exist");
                    throw new System.InvalidProgramException();
                }
                office.TryReplaceInfo(officeInfoWithManyEmployees.Id);
                int maxEmployees = officeInfoWithManyEmployees.MaximumEmployees;
                Assert.IsTrue(office.CanHireEmployee());
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.AreEqual(1, office.Divisions.GetEmployeesCount());

                if (recruit.Employee is ISingleEmployee)
                {
                    shop.Cart.Add(recruit, 1);
                    Assert.IsTrue(!shop.Cart.CanPurchaseCart());
                    return;
                }

                for (int i = 0; i < maxEmployees - 1; ++i)
                {
                    shop.Cart.Add(recruit, 1);
                    Assert.IsTrue(shop.Cart.CanPurchaseCart());
                    shop.Cart.PurchaseCart();
                }
                Assert.IsTrue(office.Divisions.GetEmployeesCount() == maxEmployees);
                shop.Cart.Add(recruit, 1);
                Assert.IsTrue(!shop.Cart.CanPurchaseCart());
            }

            private void DoRentablePremiseCartPurchasePositiveTest<ShopItem, Premise>(Func<BrowserData, ShopCartData<ShopItem>> getShop, Func<CompanyData, RentablePremiseData> getPremise)
                where ShopItem : RentablePremiseShopItemData<Premise>, ICloneable<ShopItem>, ISingleShopItem
                where Premise : RentablePremise
            {
                PrepareCartWithItem(0, 1, getShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
                Assert.IsTrue(shop.Cart.CanPurchaseCart());
                shop.Cart.PurchaseCart();
                Assert.IsTrue(getPremise.Invoke(GameData.Data.CompanyData).Info.Id == item.Info.PremiseInfo.Id);
            }

            private void DoCartAddRemoveItemPositiveTest<ShopItem>(Func<BrowserData, ShopCartData<ShopItem>> getShop) where ShopItem : ShopItemData, ICloneable<ShopItem>
            {
                PrepareCartWithItem(0, 1, getShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);

                shop.Cart.Add(item, 2);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(3, shop.Cart.Items[0].Count);

                Assert.IsTrue(shop.Items.Exists(x => x.Id == 1, out item));
                shop.Cart.Add(item, 0);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                shop.Cart.Add(item, 1);
                Assert.AreEqual(2, shop.Cart.Items.Count);
                shop.Cart.Add(item, 3);
                Assert.AreEqual(2, shop.Cart.Items.Count);
                Assert.AreEqual(3, shop.Cart.Items[0].Count);
                Assert.AreEqual(4, shop.Cart.Items[1].Count);

                shop.Cart.Remove(item, 2);
                Assert.AreEqual(2, shop.Cart.Items.Count);
                Assert.AreEqual(2, shop.Cart.Items[1].Count);
                shop.Cart.Remove(item, 1);
                Assert.AreEqual(2, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[1].Count);
                shop.Cart.Remove(item, 1);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(3, shop.Cart.Items[0].Count);
            }
            private void DoCartAddRemoveItemNegativeTest<ShopItem>(Func<BrowserData, ShopCartData<ShopItem>> getShop) where ShopItem : ShopItemData, ICloneable<ShopItem>
            {
                PrepareCartWithItem(0, 0, getShop, out var shop, out var item);
                Assert.AreEqual(0, shop.Cart.Items.Count);

                shop.Cart.Add(item, -10);
                Assert.AreEqual(0, shop.Cart.Items.Count);

                shop.Cart.Add(item, 10);
                shop.Cart.Remove(item, -10);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(10, shop.Cart.Items[0].Count);

                shop.Cart.Remove(item, 100);
                Assert.AreEqual(0, shop.Cart.Items.Count);
            }

            private void DoCartSingleAddRemoveItemPositiveTest<ShopItem>(Func<BrowserData, ShopCartData<ShopItem>> getShop) where ShopItem : ShopItemData, ICloneable<ShopItem>, ISingleShopItem
            {
                PrepareCartWithItem(0, 1, getShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);

                shop.Cart.Remove(item, 1);
                Assert.AreEqual(0, shop.Cart.Items.Count);
            }
            private void DoCartSingleAddRemoveItemNegativeTest<ShopItem>(Func<BrowserData, ShopCartData<ShopItem>> getShop) where ShopItem : ShopItemData, ICloneable<ShopItem>, ISingleShopItem
            {
                PrepareCartWithItem(0, 1, getShop, out var shop, out var item);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                shop.Cart.Add(item, 14);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);

                shop.Cart.Remove(item, 1);
                Assert.AreEqual(0, shop.Cart.Items.Count);

                shop.Cart.Add(item, 14);
                Assert.AreEqual(1, shop.Cart.Items.Count);
                Assert.AreEqual(1, shop.Cart.Items[0].Count);
            }

            private void PrepareCartWithItem<T>(int id, int count, Func<BrowserData, ShopCartData<T>> getShop, out ShopCartData<T> shop, out T item) where T : ShopItemData, ICloneable<T>
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                GameData.Data.PlayerData.Wallet.TryIncreaseValue(100000);
                shop = getShop.Invoke(GameData.Data.BrowserData);
                shop.GenerateNewData();
                shop.Items.Exists(x => x.Id == id, out item);
                shop.Cart.Add(item, count);
            }
            #endregion methods
        }
    }
}