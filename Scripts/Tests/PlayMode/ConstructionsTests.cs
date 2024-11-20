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
        public class ConstructionsTests
        {
            #region fields & properties


            #endregion fields & properties

            #region methods
            [Test]
            public void TryCancelBuildPositiveTest()
            {
                PrepareAnyTest(out ConstructionsData constructions);
                GameData.Data.CompanyData.OfficeData.TryReplaceInfo(0);
                GameData.Data.CompanyData.OfficeData.TryHireBuilder(new(0));
                BuilderData builder = (BuilderData)GameData.Data.CompanyData.OfficeData.Divisions.Builders.Employees[0];
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("1", 0, 0), GetEmptyRooms()));
                ConstructionData construction = constructions.Constructions[0];
                Assert.IsTrue(construction.TryAddBuilder(builder));
                Assert.AreEqual(1, construction.Builders.Count);
                Assert.IsTrue(constructions.TryCancelBuild(0));
                Assert.IsFalse(construction.IsBuilded);
                Assert.AreEqual(0, construction.Builders.Count);
            }
            [Test]
            public void TryCancelBuildNegativeTest()
            {
                PrepareAnyTest(out ConstructionsData constructions);
                GameData.Data.CompanyData.OfficeData.TryReplaceInfo(0);
                GameData.Data.CompanyData.OfficeData.TryHireBuilder(new(0));
                BuilderData builder = (BuilderData)GameData.Data.CompanyData.OfficeData.Divisions.Builders.Employees[0];
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("1", 0, 0), GetEmptyRooms()));
                ConstructionData construction = constructions.Constructions[0];
                Assert.IsTrue(construction.TryAddBuilder(builder));
                Assert.IsFalse(constructions.TryCancelBuild(-1));
                Assert.IsFalse(constructions.TryCancelBuild(1));
                Assert.AreEqual(1, construction.Builders.Count);
                Assert.IsTrue(constructions.TryBuildByBaseId(0));
                Assert.IsFalse(constructions.TryCancelBuild(0));
                Assert.AreEqual(0, construction.Builders.Count);
            }
            [Test]
            public void TryGetPositiveTest()
            {
                PrepareAnyTest(out ConstructionsData constructions);
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("1", 0, 0), GetEmptyRooms()));
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("0", 1, 1), GetEmptyRooms()));
                Assert.IsTrue(constructions.TryGet(0, out _));
                Assert.IsTrue(constructions.TryGet(1, out _));
            }
            [Test]
            public void TryGetNegativeTest()
            {
                PrepareAnyTest(out ConstructionsData constructions);
                Assert.IsFalse(constructions.TryGet(0, out _));
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("3", 0, 0), GetEmptyRooms()));
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("0", 1, 1), GetEmptyRooms()));
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("2", 2, 2), GetEmptyRooms()));
                Assert.IsFalse(constructions.TryGet(-1, out _));
                Assert.IsFalse(constructions.TryGet(3, out _));
                Assert.IsFalse(constructions.TryGet(4, out _));
            }

            [Test]
            public void AddPositiveTest()
            {
                PrepareAnyTest(out ConstructionsData constructions);
                Assert.AreEqual(0, constructions.Constructions.Count);
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("1", 0, 0), GetEmptyRooms()));
                Assert.AreEqual(1, constructions.Constructions.Count);
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("0", 1, 1), GetEmptyRooms()));
                Assert.AreEqual(2, constructions.Constructions.Count);
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("0", 2, 2), GetEmptyRooms()));
                Assert.AreEqual(3, constructions.Constructions.Count);
            }
            [Test]
            public void AddNegativeTest()
            {
                PrepareAnyTest(out ConstructionsData constructions);
                Assert.AreEqual(0, constructions.Constructions.Count);
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("1", 0, 0), GetEmptyRooms()));
                Assert.IsFalse(constructions.TryAdd(GetEmptyBlueprint("1", 1, 0), GetEmptyRooms()));
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("1", -1, 1), GetEmptyRooms()));
                Assert.IsFalse(constructions.TryAdd(GetEmptyBlueprint("1", 1, -1), GetEmptyRooms()));
                Assert.AreEqual(2, constructions.Constructions.Count);
            }
            private List<BlueprintRoomData> GetEmptyRooms() => new();
            private BlueprintData GetEmptyBlueprint(string name, int id, int baseId)
            {
                BlueprintData bp = new(name, id, new List<BlueprintResourceData>());
                bp.ChangeBaseId(baseId);
                return bp;
            }
            private void PrepareAnyTest(out ConstructionsData constructions)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                constructions = GameData.Data.ConstructionsData;
            }
            #endregion methods
        }
    }
}