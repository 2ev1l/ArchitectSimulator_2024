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
        public class ReviewTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void GeneratePositiveTest()
            {
                PrepareAnyTest(out ReviewsData reviews);
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                ConstructionsData constructions = GameData.Data.ConstructionsData;
                GameData.Data.CompanyData.Rating.TryIncreaseValue(10);
                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.IsTrue(constructions.TryAdd(GetEmptyBlueprint("0", 0), new()));
                Assert.IsTrue(tasks.TryAcceptTask(0, 0));
                Assert.IsTrue(tasks.TrySetTaskExpired(0));
                ReviewData review = reviews.TryAdd(tasks.ExpiredTasks[0]);
                Assert.AreNotSame(null, review);
                Assert.AreNotSame(null, review.Task);
                Assert.IsFalse(review.IsTaskCompleted);
                Assert.IsTrue(review.GameTextIds.Count > 0);
                Assert.IsTrue(review.GetDescription().Length > 0);
            }
            [Test]
            public void GenerateNegativeTest()
            {
                PrepareAnyTest(out ReviewsData reviews);
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                ConstructionsData constructions = GameData.Data.ConstructionsData;
                tasks.TryStartTask(0);
                ReviewData review = reviews.TryAdd(tasks.StartedTasks[0]);
                Assert.AreSame(null, review);
            }
            [Test]
            public void AddPositiveTest()
            {
                PrepareAnyTest(out ReviewsData reviews);
                Assert.AreEqual(0, reviews.Reviews.Count);
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                ConstructionsData constructions = GameData.Data.ConstructionsData;
                tasks.TryStartTask(0);
                constructions.TryAdd(GetEmptyBlueprint("0", 0), new());
                tasks.TryAcceptTask(0, 0);
                Assert.AreSame(reviews.TryAdd(tasks.StartedTasks[0]), reviews.Reviews[0]);
            }
            [Test]
            public void AddNegativeTest()
            {
                PrepareAnyTest(out ReviewsData reviews);
                ReviewData review = reviews.TryAdd(null);
                Assert.AreSame(review, null);
            }
            private BlueprintData GetEmptyBlueprint(string name, int blueprintId) => new(name, blueprintId, new List<BlueprintResourceData>());
            private void PrepareAnyTest(out ReviewsData reviews)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                reviews = GameData.Data.CompanyData.ReviewsData;
            }
            #endregion methods
        }
    }
}