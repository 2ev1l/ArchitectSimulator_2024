using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Events;

namespace Tests.PlayMode
{
    public partial class WorldDataTests
    {
        public class TaskTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void AddRewardPositiveConstructionTasksTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                RewardRequestExecutor rewardRequestExecutor = new();
                rewardRequestExecutor.Initialize();
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                Assert.IsTrue(tasks.TryStartTask(0));
                tasks.StartedTasks[0].Info.RewardInfo.HasReward(RewardType.Rating, out Reward ratingReward);
                int ratingIncrease = ratingReward.Value;
                Assert.IsTrue(ratingIncrease > 0);
                int oldRating = GameData.Data.CompanyData.Rating.Value;
                Assert.IsTrue(tasks.TryCompleteTask(0));
                Assert.AreEqual(oldRating + ratingIncrease, GameData.Data.CompanyData.Rating.Value);
                rewardRequestExecutor.Dispose();
            }
            [Test]
            public void StartCompleteExpirePositivePlayerTasksTest() => DoStartCompleteExpirePositiveTest(x => x.PlayerData.Tasks, x => x.PlayerTaskInfo.Data.Select(x => x.Data));
            [Test]
            public void StartCompleteExpireNegativePlayerTasksTest() => DoStartCompleteExpireNegativeTest(x => x.PlayerData.Tasks, x => x.PlayerTaskInfo.Data.Select(x => x.Data));

            [Test]
            public void GenerateConstructionTasksPositiveTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                GameData.Data.CompanyData.Rating.TryIncreaseValue(10);
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                tasks.GenerateAcceptableTasks(out var generated);
                Assert.IsTrue(generated.Count > 1);
                int countToAdd = 2;
                tasks.StartGeneratedTasks(countToAdd, generated);
                Assert.IsTrue(tasks.StartedTasks.Count == countToAdd);

                ConstructionsData constructions = GameData.Data.ConstructionsData;
                int c = 0;
                foreach (var el in generated)
                {
                    BlueprintData bp = new("", el.BlueprintInfo.Id, new List<BlueprintResourceData>());
                    bp.ChangeBaseId(c);
                    constructions.TryAdd(bp, new List<BlueprintRoomData>());
                    c++;
                }
                int startGeneratedCount = generated.Count;
                tasks.RemoveGeneratedCompletedTasks(countToAdd, generated);
                Assert.IsTrue(generated.Count >= countToAdd);
                int safety = 100;
                for (int i = 0; i < safety; ++i)
                {
                    tasks.RemoveGeneratedCompletedTasks(countToAdd, generated);
                }
                Assert.IsTrue(generated.Count < startGeneratedCount);
                Assert.IsTrue(generated.Count >= countToAdd);
            }
            [Test]
            public void GenerateConstructionTasksNegativeTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                GameData.Data.CompanyData.Rating.TryIncreaseValue(10);
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                tasks.GenerateAcceptableTasks(out var generated);
                tasks.StartGeneratedTasks(0, generated);
                Assert.IsTrue(tasks.StartedTasks.Count == 0);
                tasks.StartGeneratedTasks(-1, generated);
                Assert.IsTrue(tasks.StartedTasks.Count == 0);
            }
            [Test]
            public void StartCompleteExpirePositiveConstructionTasksTest()
            {
                DoStartCompleteExpirePositiveTest(x => x.CompanyData.ConstructionTasks, x => x.ConstructionTaskInfo.Data.Select(x => x.Data));
                GameData.SetData(new());
                BlueprintsData blueprints = GameData.Data.BlueprintsData;
                ConstructionsData constructions = GameData.Data.ConstructionsData;
                Assert.AreEqual(0, GameData.Data.CompanyData.Rating.Value);
                GameData.Data.CompanyData.Rating.TryIncreaseValue(10);
                int companyRating = GameData.Data.CompanyData.Rating.Value;
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;

                tasks.TryStartTask(0);
                Assert.IsTrue(tasks.StartedTasks[0].BlueprintBaseIdReference < 0);
                Assert.IsTrue(tasks.TryAcceptTask(0, 0));
                BlueprintInfo blueprintInfoForTask0 = DB.Instance.ConstructionTaskInfo[0].Data.BlueprintInfo;
                Assert.IsTrue(blueprints.TryAddNewBlueprint(new("1", blueprintInfoForTask0.Id, new List<BlueprintResourceData>()), out _));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.StartedTasks[0].BlueprintBaseIdReference);
                Assert.IsTrue(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.AreEqual(companyRating - 1, GameData.Data.CompanyData.Rating.Value);

                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.IsTrue(tasks.TryStartTask(1));

                Assert.IsTrue(blueprints.TryAddNewBlueprint(new("1", blueprintInfoForTask0.Id, new List<BlueprintResourceData>()), out _));
                BlueprintData bp = blueprints.Blueprints[0];
                Assert.IsTrue(tasks.TryAcceptTask(1, bp.BaseId));
                Assert.IsTrue(constructions.TryAdd(bp, new()));
                Assert.IsTrue(blueprints.TryRemoveBlueprint("1"));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.AreEqual(1, constructions.Constructions.Count);

                Assert.IsTrue(tasks.TrySetTaskExpired(1));
                Assert.AreEqual(0, constructions.Constructions.Count);

            }
            [Test]
            public void StartCompleteExpireNegativeConstructionTasksTest()
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                Assert.AreEqual(0, tasks.TaskCountInLists(0));
                Assert.IsFalse(tasks.TryStartTask(-1));

                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.IsFalse(tasks.TryStartTask(0));
                Assert.IsFalse(tasks.TryStartTask(-1));
                Assert.AreEqual(1, tasks.TaskCountInLists(0));

                Assert.IsTrue(tasks.TryStartTask(1));
                Assert.IsTrue(tasks.TryCompleteTask(0));
                Assert.IsFalse(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(1, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.TaskCountInLists(0));
                Assert.AreEqual(1, tasks.TaskCountInLists(1));

                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.IsTrue(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(2, tasks.TaskCountInLists(0));

                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.IsTrue(tasks.TryCompleteTask(0));
                Assert.AreEqual(3, tasks.TaskCountInLists(0));

                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.IsTrue(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(2, tasks.ExpiredTasks.Count);
                Assert.AreEqual(2, tasks.CompletedTasks.Count);
                Assert.AreEqual(4, tasks.TaskCountInLists(0));

                GameData.SetData(new());
                tasks = GameData.Data.CompanyData.ConstructionTasks;
                BlueprintsData blueprints = GameData.Data.BlueprintsData;
                Assert.AreEqual(0, GameData.Data.CompanyData.Rating.Value);
                GameData.Data.CompanyData.Rating.TryIncreaseValue(10);
                int companyRating = GameData.Data.CompanyData.Rating.Value;
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                tasks.TryStartTask(0);
                Assert.IsTrue(tasks.StartedTasks[0].BlueprintBaseIdReference < 0);
                Assert.IsFalse(tasks.TryAcceptTask(-1, 0));
                Assert.IsFalse(tasks.TryAcceptTask(0, -1));
                BlueprintInfo blueprintInfoForTask0 = DB.Instance.ConstructionTaskInfo[0].Data.BlueprintInfo;
                Assert.IsTrue(blueprints.TryAddNewBlueprint(new("1", blueprintInfoForTask0.Id, new List<BlueprintResourceData>()), out _));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsTrue(tasks.TryAcceptTask(0, 0));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsFalse(tasks.TryAcceptTask(0, 0));
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.AreEqual(1, tasks.StartedTasks.Count);

                Assert.IsFalse(tasks.TrySetTaskExpired(-1));
                Assert.AreEqual(companyRating, GameData.Data.CompanyData.Rating.Value);
                Assert.IsFalse(tasks.TrySetTaskExpired(1));
                Assert.AreEqual(companyRating, GameData.Data.CompanyData.Rating.Value);
                Assert.AreEqual(1, blueprints.Blueprints.Count);
                Assert.IsTrue(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.IsFalse(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(0, blueprints.Blueprints.Count);
                Assert.AreEqual(companyRating - 1, GameData.Data.CompanyData.Rating.Value);
            }

            private void DoStartCompleteExpirePositiveTest<Task, Info>(Func<GameData, TasksData<Task, Info>> getTasks, Func<DB, IEnumerable<Info>> getDBSet)
                where Task : TaskData<Info>
                where Info : TaskInfo
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                var tasks = getTasks.Invoke(GameData.Data);
                var dbSet = getDBSet.Invoke(DB.Instance);
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);
                Assert.AreEqual(0, tasks.StartedTasks[0].Id);

                Assert.IsTrue(tasks.TryStartTask(1));
                Assert.AreEqual(2, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);
                Assert.AreEqual(1, tasks.StartedTasks[1].Id);

                Assert.IsTrue(tasks.TryCompleteTask(0));
                Assert.AreEqual(1, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks[0].Id);

                Assert.IsTrue(tasks.TrySetTaskExpired(1));
                Assert.AreEqual(1, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks[0].Id);
            }
            private void DoStartCompleteExpireNegativeTest<Task, Info>(Func<GameData, TasksData<Task, Info>> getTasks, Func<DB, IEnumerable<Info>> getDBSet)
                where Task : TaskData<Info>
                where Info : TaskInfo
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                var tasks = getTasks.Invoke(GameData.Data);
                var dbSet = getDBSet.Invoke(DB.Instance);

                Assert.IsFalse(tasks.TryStartTask(-1));
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsTrue(tasks.TryStartTask(0));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TryStartTask(0));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TryCompleteTask(-1));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TryCompleteTask(1));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TrySetTaskExpired(1));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TrySetTaskExpired(-1));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(0, tasks.ExpiredTasks.Count);

                Assert.IsTrue(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TrySetTaskExpired(0));
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TryStartTask(0));
                Assert.AreEqual(0, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);

                Assert.IsTrue(tasks.TryStartTask(1));
                Assert.AreEqual(1, tasks.StartedTasks.Count);
                Assert.AreEqual(0, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);

                Assert.IsTrue(tasks.TryCompleteTask(1));
                Assert.AreEqual(1, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);

                Assert.IsFalse(tasks.TryCompleteTask(1));
                Assert.AreEqual(1, tasks.CompletedTasks.Count);
                Assert.AreEqual(1, tasks.ExpiredTasks.Count);

            }
            #endregion methods
        }
    }
}