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
        public class BlueprintsTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void BaseIdChangeTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                blueprints.TryAddNewBlueprint(GetEmptyBlueprint("name1", 0));
                Assert.AreEqual(0, blueprints.Blueprints[0].BaseId);

                blueprints.TryAddNewBlueprint(GetEmptyBlueprint("name2", 24));
                Assert.AreEqual(1, blueprints.Blueprints[1].BaseId);
                blueprints.TryAddNewBlueprint(GetEmptyBlueprint("name3", 333));
                Assert.AreEqual(2, blueprints.Blueprints[2].BaseId);
                blueprints.TryRemoveBlueprint(1);
                blueprints.TryRemoveBlueprint(2);

                blueprints.TryAddNewBlueprint(GetEmptyBlueprint("name3", 333));
                Assert.AreEqual(3, blueprints.Blueprints[1].BaseId);
            }

            [Test]
            public void RemoveStringPositiveTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.IsTrue(blueprints.TryRemoveBlueprint("1"));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("2", 1)));
                Assert.AreEqual(2, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryRemoveBlueprint("2"));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryRemoveBlueprint("1"));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
            }
            [Test]
            public void RemoveStringNegativeTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.IsFalse(blueprints.TryRemoveBlueprint("-0"));
                Assert.IsFalse(blueprints.TryRemoveBlueprint("0"));
                Assert.IsFalse(blueprints.TryRemoveBlueprint("-1"));
                Assert.IsFalse(blueprints.TryRemoveBlueprint("a"));

                Assert.IsTrue(blueprints.TryRemoveBlueprint("1"));
                Assert.IsFalse(blueprints.TryRemoveBlueprint("1"));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
            }

            [Test]
            public void RemoveIdPositiveTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.IsTrue(blueprints.TryRemoveBlueprint(0));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("2", 1)));
                Assert.AreEqual(2, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryRemoveBlueprint(1));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryRemoveBlueprint(2));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
            }
            [Test]
            public void RemoveIdNegativeTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.IsFalse(blueprints.TryRemoveBlueprint(-1));
                Assert.IsFalse(blueprints.TryRemoveBlueprint(1));

                Assert.IsTrue(blueprints.TryRemoveBlueprint(0));
                Assert.IsFalse(blueprints.TryRemoveBlueprint(0));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
            }

            [Test]
            public void AddPositiveTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("0", 1)));
                Assert.AreEqual(2, blueprints.Blueprints.Count);
            }
            [Test]
            public void AddNegativeTest()
            {
                PrepareAnyTest(out BlueprintsData blueprints);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsFalse(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 1)));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsFalse(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", 0)));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsFalse(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("1", -1)));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsTrue(blueprints.TryAddNewBlueprint(GetEmptyBlueprint("0", 0)));
                Assert.AreEqual(2, blueprints.Blueprints.Count);
            }
            private BlueprintData GetEmptyBlueprint(string name, int id) => new(name, id, new List<BlueprintResourceData>());
            private void PrepareAnyTest(out BlueprintsData blueprints)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                blueprints = GameData.Data.BlueprintsData;
            }
            #endregion methods
        }
    }
}