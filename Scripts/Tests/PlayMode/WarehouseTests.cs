using Game.DataBase;
using Game.Serialization.World;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Tests.PlayMode
{
    public partial class WorldDataTests
    {
        public class WarehouseTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void ReplaceInfoPositiveTest()
            {
                AssetLoader.InitInstances();
                WarehouseData warehouseData = new(-1);
                Assert.IsTrue(warehouseData.Info == null);
                Assert.IsTrue(warehouseData.CanReplaceInfo(0));
                Assert.IsTrue(warehouseData.TryReplaceInfo(0));
                Assert.AreEqual(0, warehouseData.Id);
                Assert.AreEqual(0, warehouseData.Info.Id);
                WarehouseInfo infoWithLessVolume = (WarehouseInfo)warehouseData.Info;
                Assert.IsTrue(warehouseData.TryReplaceInfo(1));
                WarehouseInfo infoWithGreaterVolume = (WarehouseInfo)warehouseData.Info;
                if (infoWithLessVolume.Space > infoWithGreaterVolume.Space)
                {
                    (infoWithLessVolume, infoWithGreaterVolume) = (infoWithGreaterVolume, infoWithLessVolume);
                }
                else if (infoWithLessVolume.Space.Approximately(infoWithGreaterVolume.Space))
                {
                    Debug.LogError("Can't do test with same warehouse space");
                    Assert.IsTrue(false);
                    throw new System.InvalidProgramException();
                }
                Assert.IsTrue(warehouseData.TryReplaceInfo(infoWithLessVolume.Id));
                ConstructionResourceData res = new(0);
                float resVolume = res.GetTotalVolumeM3();
                int countToFill = Mathf.FloorToInt(infoWithLessVolume.Space / resVolume);
                res.Add(countToFill - 1);
                Assert.IsTrue(warehouseData.TryAddConstructionResource(res));
                Assert.IsTrue(warehouseData.TryReplaceInfo(infoWithGreaterVolume.Id));
            }
            [Test]
            public void ReplaceInfoNegativeTest()
            {
                AssetLoader.InitInstances();
                WarehouseData warehouseData = new(-53189);
                Assert.IsTrue(warehouseData.CanReplaceInfo(0));
                Assert.IsTrue(warehouseData.TryReplaceInfo(0));
                Assert.AreEqual(0, warehouseData.Id);
                WarehouseInfo infoWithLessVolume = (WarehouseInfo)warehouseData.Info;
                Assert.IsTrue(warehouseData.TryReplaceInfo(1));
                Assert.IsTrue(warehouseData.Info.Id == 1);
                WarehouseInfo infoWithGreaterVolume = (WarehouseInfo)warehouseData.Info;
                if (infoWithLessVolume.Space > infoWithGreaterVolume.Space)
                {
                    (infoWithLessVolume, infoWithGreaterVolume) = (infoWithGreaterVolume, infoWithLessVolume);
                }
                else if (infoWithLessVolume.Space.Approximately(infoWithGreaterVolume.Space))
                {
                    Debug.LogError("Can't do test with same warehouse space");
                    throw new System.InvalidProgramException();
                }
                warehouseData.TryReplaceInfo(infoWithGreaterVolume.Id);
                ConstructionResourceData res = new(0);
                float resVolume = res.GetTotalVolumeM3();
                int countToFill = Mathf.FloorToInt(infoWithGreaterVolume.Space / resVolume);
                res.Add(countToFill - 1);
                warehouseData.TryAddConstructionResource(res);
                Assert.IsTrue(warehouseData.FreeSpace < resVolume);
                if (warehouseData.OccupiedSpace < infoWithLessVolume.Space)
                {
                    Debug.LogError("Can't do test because space isn't filled");
                    return;
                }
                Assert.IsTrue(!warehouseData.TryReplaceInfo(infoWithLessVolume.Id));
            }

            [Test]
            public void AddRemoveResourcePositiveTest()
            {
                AssetLoader.InitInstances();

                WarehouseData warehouseData = new(0);
                ConstructionResourceData cr = new(0);
                cr.Add(1);
                float rVolume = cr.GetTotalVolumeM3();
                float newSpace = warehouseData.OccupiedSpace + rVolume;
                Assert.IsTrue(warehouseData.TryAddConstructionResource(cr));
                Assert.AreSame(warehouseData.ConstructionResources.Items[0], cr);
                Assert.IsTrue(warehouseData.OccupiedSpace == newSpace);

                cr = new(0);
                cr.Add(2);
                rVolume = cr.GetTotalVolumeM3();
                newSpace = warehouseData.OccupiedSpace + rVolume;
                Assert.IsTrue(warehouseData.TryAddConstructionResource(cr));
                Assert.IsTrue(warehouseData.OccupiedSpace == newSpace);
                Assert.IsTrue(warehouseData.ConstructionResources.Items.Count == 1);

                cr = new(1);
                cr.Add(2);
                cr.Add(1);
                rVolume = cr.GetTotalVolumeM3();
                newSpace = warehouseData.OccupiedSpace + rVolume;
                Assert.IsTrue(warehouseData.TryAddConstructionResource(cr));
                Assert.AreSame(cr, warehouseData.ConstructionResources.Items[1]);
                Assert.IsTrue(warehouseData.OccupiedSpace == newSpace);
                Assert.IsTrue(warehouseData.ConstructionResources.Items.Count == 2);

                rVolume = 3 * cr.Info.Prefab.VolumeM3;
                newSpace = warehouseData.OccupiedSpace - rVolume;
                warehouseData.RemoveConstructionResource(cr, 3);

                Assert.AreEqual(newSpace, warehouseData.OccupiedSpace);
                Assert.IsTrue(warehouseData.ConstructionResources.Items.Count == 2);

                warehouseData.RemoveConstructionResource(cr, 1);
                Assert.IsTrue(warehouseData.ConstructionResources.Items.Count == 1);
            }
            [Test]
            public void AddRemoveResourceNegativeTest()
            {
                AssetLoader.InitInstances();
                WarehouseData warehouseData = new(-1);
                Assert.IsTrue(!warehouseData.CanAddResource(0));
                Assert.IsTrue(!warehouseData.CanAddResource(1));
                Assert.IsTrue(!warehouseData.CanAddResource(100));
                warehouseData.TryReplaceInfo(0);

                float testVolume = 100000;

                Assert.IsTrue(!warehouseData.CanAddResource(testVolume));
                int addAmount = Mathf.FloorToInt(testVolume / DB.Instance.ConstructionResourceInfo.Data[0].Data.Prefab.VolumeM3);
                Assert.IsTrue(!warehouseData.TryAddConstructionResource(0, addAmount));
                Assert.AreEqual(0, warehouseData.ConstructionResources.Items.Count);

                warehouseData.TryAddConstructionResource(0, 1);
                warehouseData.RemoveConstructionResource(0, 0);
                Assert.AreEqual(1, warehouseData.ConstructionResources.Items.Count);
                warehouseData.RemoveConstructionResource(0, -100);
                Assert.AreEqual(1, warehouseData.ConstructionResources.Items.Count);
                warehouseData.RemoveConstructionResource(0, 100);
                Assert.AreEqual(0, warehouseData.ConstructionResources.Items.Count);
            }

            [Test]
            public void AddMultipleResourcesPositiveTest()
            {
                AssetLoader.InitInstances();

                WarehouseData warehouseData = new(0);
                List<ConstructionResourceData> resources = new();
                ConstructionResourceData cr = new(0);
                cr.Add(1);
                resources.Add(cr);

                cr = new(1);
                cr.Add(2);
                cr.Add(1);
                resources.Add(cr);

                float rVolume = 0;
                foreach (var el in resources)
                    rVolume += el.GetTotalVolumeM3();

                float newSpace = warehouseData.OccupiedSpace + rVolume;

                Assert.IsTrue(warehouseData.CanAddResources(resources, out float floatVolume));
                Assert.IsTrue(floatVolume.Approximately(rVolume));
                Assert.IsTrue(warehouseData.TryAddConstructionResources(resources));
                Assert.IsTrue(warehouseData.OccupiedSpace == newSpace);
                Assert.AreSame(warehouseData.ConstructionResources.Items[1], cr);
                Assert.AreEqual(2, warehouseData.ConstructionResources.Items.Count);
            }
            [Test]
            public void AddMultipleResourcesNegativeTest()
            {
                AssetLoader.InitInstances();

                WarehouseData warehouseData = new(0);
                List<ConstructionResourceData> resources = new();
                ConstructionResourceData cr = new(0);
                cr.Add(1);
                resources.Add(cr);

                cr = new(1);
                cr.Add(2);
                cr.Add(100000);
                resources.Add(cr);

                float rVolume = 0;
                foreach (var el in resources)
                    rVolume += el.GetTotalVolumeM3();

                float newSpace = warehouseData.OccupiedSpace + rVolume;
                Assert.IsFalse(warehouseData.CanAddResources(resources, out float floatVolume));
                Assert.IsTrue(floatVolume == rVolume);
                Assert.IsFalse(warehouseData.TryAddConstructionResources(resources));
                Assert.IsFalse(warehouseData.OccupiedSpace == newSpace);
                Assert.AreEqual(0, warehouseData.ConstructionResources.Items.Count);
            }
            #endregion methods
        }
    }
}