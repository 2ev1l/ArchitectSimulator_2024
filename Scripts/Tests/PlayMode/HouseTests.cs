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
    public partial class WorldDataTests
    {
        public class HouseTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void MoodScaleTest()
            {
                PrepareAnyTest(out HouseData house);
                Assert.AreEqual(1, house.MoodScale);
                HouseInfo info = DB.Instance.HouseInfo[1].Data;
                house.TryReplaceInfo(1);
                Assert.AreEqual(info.MoodScale, house.MoodScale);
            }
            [Test]
            public void AddRemovePeoplePositiveTest()
            {
                PrepareAnyTest(out HouseData house);
                HouseInfoSO houseWithManyPeople = DB.Instance.HouseInfo.Find(x => x.Data.MaxPeople > 1);
                if (houseWithManyPeople == null)
                {
                    Debug.LogError("Can't do test because can't find house with people count > 1");
                    throw new System.InvalidProgramException();
                }
                Assert.IsTrue(house.TryReplaceInfo(houseWithManyPeople.Id));
                Assert.AreEqual(1, house.CurrentPeopleCount);
                Assert.IsTrue(house.TryAddPeople());
                Assert.AreEqual(2, house.CurrentPeopleCount);
                Assert.IsTrue(house.TryRemovePeople());
                Assert.AreEqual(1, house.CurrentPeopleCount);
            }
            [Test]
            public void AddRemovePeopleNegativeTest()
            {
                PrepareAnyTest(out HouseData house);
                Assert.AreEqual(1, house.CurrentPeopleCount);
                Assert.IsFalse(house.TryAddPeople());
                Assert.AreEqual(1, house.CurrentPeopleCount);
                Assert.IsFalse(house.TryRemovePeople());
                Assert.AreEqual(1, house.CurrentPeopleCount);
                HouseInfoSO houseWithManyPeople = DB.Instance.HouseInfo.Find(x => x.Data.MaxPeople > 1);
                if (houseWithManyPeople == null)
                {
                    Debug.LogError("Can't do test because can't find house with people count > 1");
                    throw new System.InvalidProgramException();
                }
                Assert.IsTrue(house.TryReplaceInfo(houseWithManyPeople.Id));
                Assert.IsTrue(house.TryAddPeople());
                Assert.IsFalse(house.TryReplaceInfo(0));
            }

            private void PrepareAnyTest(out HouseData house)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                house = GameData.Data.PlayerData.HouseData;
            }
            #endregion methods

        }
    }
}
