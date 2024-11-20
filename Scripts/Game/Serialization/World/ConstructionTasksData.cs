using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal.Collections.Generic;
using Universal.Core;
using static Game.Serialization.World.ConstructionTaskData;

namespace Game.Serialization.World
{
    /// <summary>
    /// Start -> Accept or Reject -> Complete
    /// </summary>
    [System.Serializable]
    public class ConstructionTasksData : TasksData<ConstructionTaskData, ConstructionTaskInfo>
    {
        #region fields & properties
        public UnityAction<ConstructionTaskData> OnTaskRejected;
        public UnityAction<ConstructionTaskData> OnTaskAccepted;
        #endregion fields & properties

        #region methods
        public override void OnMonthUpdate(MonthData monthData)
        {
            CheckTasksRejection();
            base.OnMonthUpdate(monthData);
            TryGenerateNewTasks(monthData);
        }
        private void CheckTasksRejection()
        {
            int itemsCount = StartedTasks.Count;
            List<ConstructionTaskData> tasksToReject = new();
            for (int i = itemsCount - 1; i >= 0; --i)
            {
                ConstructionTaskData startedTask = StartedTasks[i];
                if (!startedTask.IsExpired()) continue;
                if (startedTask.IsAccepted) continue;
                //expiration sets in base
                tasksToReject.Add(startedTask);
            }
            foreach (var task in tasksToReject)
            {
                RejectTask(task);
            }
        }
        private void TryGenerateNewTasks(MonthData monthData)
        {
            if (monthData.CurrentMonth < 3) return;
            int countToAdd = 1;

            countToAdd += GetMonthTasksIncrease(monthData);
            countToAdd += GetPRTasksIncrease();

            if (countToAdd == 0) return;
            GenerateAcceptableTasks(out List<ConstructionTaskInfo> allAcceptableTasks);
            RemoveGeneratedCompletedTasks(countToAdd, allAcceptableTasks);
            StartGeneratedTasks(countToAdd, allAcceptableTasks);
        }
        public void StartGeneratedTasks(int countToAdd, List<ConstructionTaskInfo> allAcceptableTasks)
        {
            if (countToAdd <= 0) return;
            int countAdded = 0;
            allAcceptableTasks.Shuffle();
            foreach (ConstructionTaskInfo info in allAcceptableTasks)
            {
                if (!TryStartTask(info.Id)) continue;
                countAdded++;
                if (countAdded >= countToAdd) break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allAcceptableTasks">List will be modified</param>
        public void RemoveGeneratedCompletedTasks(int countToAdd, List<ConstructionTaskInfo> allAcceptableTasks)
        {
            int maxRemove = Mathf.Min(allAcceptableTasks.Count / 2, allAcceptableTasks.Count - countToAdd);
            maxRemove = Mathf.Max(maxRemove, 0);
            if (maxRemove == 0) return;
            int currentRemove = 0;
            HashSet<int> buildedBlueprints = GameData.Data.ConstructionsData.Constructions.Select(x => x.BlueprintInfoId).ToHashSet();
            foreach (int bpId in buildedBlueprints)
            {
                if (currentRemove >= maxRemove) break;
                if (!allAcceptableTasks.Exists(x => x.BlueprintInfo.Id == bpId, out ConstructionTaskInfo exist)) continue;
                if (CustomMath.GetRandomChance(30)) continue;
                allAcceptableTasks.Remove(exist);
                currentRemove++;
            }
        }
        public void GenerateAcceptableTasks(out List<ConstructionTaskInfo> allAcceptableTasks)
        {
            int currentRating = GameData.Data.CompanyData.Rating.Value;
            float ratingScale = 1 + (currentRating / 100f);
            ratingScale = Mathf.Pow(ratingScale, 1.5f);
            Vector2 ratingScaleRange = new Vector2(-5, +3) * ratingScale;
            switch (currentRating)
            {
                case int i when i < 10: ratingScaleRange.y = 0; break;
                case int i when i >= 12 && i < 20: ratingScaleRange.x = 0; break;

            }
            Vector2 acceptableRatingRange = new Vector2(currentRating + ratingScaleRange.x, currentRating + ratingScaleRange.y);
            allAcceptableTasks = new();
            foreach (ConstructionTaskInfoSO so in DB.Instance.ConstructionTaskInfo.Data)
            {
                ConstructionTaskInfo info = so.Data;
                if (!info.IsRepeatable) continue;
                int minRating = info.MinRating;
                int maxRating = info.MaxRating;
                if (maxRating < acceptableRatingRange.x || minRating > acceptableRatingRange.y) continue;
                allAcceptableTasks.Add(info);
            }
        }
        private int GetMonthTasksIncrease(MonthData monthData)
        {
            int countToAdd = 0;
            if (monthData.CurrentMonth > 5)
                countToAdd += 1;
            if (monthData.CurrentMonth > 13)
                countToAdd -= 1;
            return countToAdd;
        }
        private int GetPRTasksIncrease()
        {
            int countToAdd = 0;
            int prRating = -1;
            ISingleEmployee prManager = GameData.Data.CompanyData.OfficeData.Divisions.PRManager.Employee;
            if (prManager != null)
            {
                prRating = prManager.SkillLevel;
            }
            if (prRating > -1)
            {
                int ratingPerTask = 10;
                countToAdd += (prRating / ratingPerTask) + 1;
                if (CustomMath.GetRandomChance(prRating + 10))
                    countToAdd += 1;
            }
            return countToAdd;
        }

        public void RejectTask(ConstructionTaskData task)
        {
            base.RemoveStartedTask(task);
            OnTaskRejected?.Invoke(task);
        }
        public int TaskCountInLists(int id)
        {
            int result = 0;
            result += TaskCountInList(base.StartedTasks, id);
            result += TaskCountInList(base.CompletedTasks, id);
            result += TaskCountInList(base.ExpiredTasks, id);
            return result;
        }
        private int TaskCountInList(IReadOnlyList<ConstructionTaskData> list, int id)
        {
            if (id < 0)
            {
                return 0;
            }
            int counter = 0;
            int itemsCount = list.Count;
            for (int i = 0; i < itemsCount; ++i)
            {
                if (list[i].Id != id) continue;
                counter++;
            }
            return counter;
        }
        public override bool CanStartTask(int id)
        {
            if (GameData.Data.CompanyData.Rating.Value < DB.Instance.ConstructionTaskInfo[id].Data.MinRating) return false;
            if (base.IsTaskStarted(id, out _)) return false;
            //don't return base
            return true;
        }
        protected override void OnBeforeTaskExpired(ConstructionTaskData expired)
        {
            base.OnBeforeTaskExpired(expired);
            int baseId = expired.BlueprintBaseIdReference;

            if (baseId > -1)
            {
                GameData.Data.CompanyData.Rating.TryDecreaseValue(1);
                GameData.Data.BlueprintsData.TryRemoveBlueprint(baseId);
                GameData.Data.ConstructionsData.TryRemove(baseId);
            }

            expired.ResetBlueprintReference();
            GameData.Data.CompanyData.ReviewsData.TryAdd(expired);
        }
        public bool TryAcceptTask(int taskId, int blueprintBaseReferenceId)
        {
            if (!IsTaskStarted(taskId, out ConstructionTaskData found)) return false;
            if (!found.TryAccept(blueprintBaseReferenceId)) return false;
            OnTaskAccepted?.Invoke(found);
            return true;
        }
        protected override void OnBeforeTaskCompleted(ConstructionTaskData task)
        {
            base.OnBeforeTaskCompleted(task);
            TaskResult taskResult = task.Result;
            if (taskResult == null) return;

            Wallet wallet = GameData.Data.PlayerData.Wallet;
            if (taskResult.MoneyChange > 0)
                wallet.TryIncreaseValue(taskResult.MoneyChange);
            else
                wallet.TryDecreaseValue(Mathf.Abs(taskResult.MoneyChange));

            MoodData mood = GameData.Data.PlayerData.Mood;
            if (taskResult.MoodChange > 0)
                mood.TryIncreaseValue(taskResult.MoodChange);
            else
                mood.TryDecreaseValue(Mathf.Abs(taskResult.MoodChange));

            CompanyData cd = GameData.Data.CompanyData;
            if (taskResult.RatingChange > 0)
                cd.Rating.TryIncreaseValue(taskResult.RatingChange);
            else
                cd.Rating.TryDecreaseValue(Mathf.Abs(taskResult.RatingChange));

            cd.ReviewsData.TryAdd(task);
        }

        protected override void AddReward(ConstructionTaskInfo taskInfo)
        {
            if (!taskInfo.RewardInfo.HasReward(RewardType.Rating, out Reward ratingReward) ||
                taskInfo.MaxRating == 0 ||
                taskInfo.MaxRating >= GameData.Data.CompanyData.Rating.Value)
            {
                base.AddReward(taskInfo);
                return;
            }
            //if max allowed rating is less than current
            List<Reward> rewards = new(taskInfo.RewardInfo.Rewards.Count - 1);
            foreach (var rew in taskInfo.RewardInfo.Rewards)
            {
                if (rew.Type == RewardType.Rating) continue;
                rewards.Add(rew);
            }

            RewardInfo reward = new(rewards.ToArray());
            reward.AddReward();
        }
        protected override ConstructionTaskData CreateNewTask(int id) => new(id);
        #endregion methods

    }
}