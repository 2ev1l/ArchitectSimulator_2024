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

        public class FoodTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void IncreaseSaturationPositiveTest()
            {
                PreapreAnyTest(out FoodData food);
                Assert.AreEqual(0, food.TotalSaturation);
                Assert.IsTrue(food.TryIncreaseSaturation(1));
                Assert.AreEqual(1, food.TotalSaturation);
                Assert.IsFalse(food.CanIncreaseSaturation);
            }
            [Test]
            public void IncreaseSaturationNegativeTest()
            {
                PreapreAnyTest(out FoodData food);
                Assert.IsFalse(food.TryIncreaseSaturation(0));
                Assert.IsFalse(food.TryIncreaseSaturation(-1));
                Assert.IsFalse(food.TryIncreaseSaturation(-10));
                Assert.AreEqual(0, food.TotalSaturation);
                Assert.IsTrue(food.TryIncreaseSaturation(1));
                Assert.IsFalse(food.TryIncreaseSaturation(1));
                Assert.AreEqual(1, food.TotalSaturation);

            }
            private void PreapreAnyTest(out FoodData food)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                food = GameData.Data.PlayerData.Food;
            }
            #endregion methods
        }
    }
}