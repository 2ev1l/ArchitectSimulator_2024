using System.Collections;
using System.Collections.Generic;
using Game.DataBase;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Events;

namespace Tests.PlayMode
{
    partial class WorldDataTests
    {

        public class VehicleTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void MoodScaleTest()
            {
                PrepareAnyTest(out VehicleData vehicle);
                Assert.AreEqual(1, vehicle.MoodScale);
                vehicle.TryReplaceInfo(0);
                VehicleInfo info = DB.Instance.VehicleInfo[0].Data;
                Assert.AreEqual(info.MoodScale, vehicle.MoodScale);
            }
            [Test]
            public void InfoChangePositiveTest()
            {
                PrepareAnyTest(out VehicleData vehicle);
                Assert.AreEqual(-1, vehicle.Id);
                Assert.IsTrue(vehicle.TryReplaceInfo(0));
                Assert.AreEqual(0, vehicle.Id);
            }
            [Test]
            public void InfoChangeNegativeTest()
            {
                PrepareAnyTest(out VehicleData vehicle);
                Assert.IsFalse(vehicle.TryReplaceInfo(-2));
                Assert.AreEqual(-1, vehicle.Id);
            }

            private void PrepareAnyTest(out VehicleData vehicle)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                vehicle = GameData.Data.PlayerData.VehicleData;
            }
            #endregion methods
        }
    }
}