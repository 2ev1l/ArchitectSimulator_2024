using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;
using static Game.Serialization.World.ConstructionData;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionTaskData : TaskData<ConstructionTaskInfo>
    {
        #region fields & properties
        public UnityAction OnClarified;

        /// <exception cref="System.NullReferenceException"></exception>
        public HumanInfo HumanInfo
        {
            get
            {
                if (humanInfoId == -1)
                {
                    if (Info.ForceHumanId)
                    {
                        humanInfoId = Info.HumanId;
                    }
                    else
                    {
                        int count = DB.Instance.HumanInfo.Data.Count;
                        humanInfoId = Random.Range(0, count);
                    }
                }
                humanInfo ??= DB.Instance.HumanInfo[humanInfoId].Data;
                return humanInfo;
            }
        }
        [System.NonSerialized] private HumanInfo humanInfo;
        [SerializeField][Min(-1)] private int humanInfoId = -1;

        /// <summary>
        /// -1 means no reference
        /// </summary>
        public int BlueprintBaseIdReference => blueprintBaseIdReference;
        [SerializeField][Min(-1)] private int blueprintBaseIdReference = -1;
        public bool IsClarified => isClarified;
        [SerializeField] private bool isClarified = false;
        /// <summary>
        /// If there's no attached construction reference, than returns null. <br></br>
        /// </summary>
        /// <exception cref="System.NullReferenceException"></exception>
        public ConstructionData ConstructionReference
        {
            get
            {
                if (blueprintBaseIdReference < 0) return null;
                if (constructionReference == null)
                {
                    if (!GameData.Data.ConstructionsData.TryGetByBaseId(blueprintBaseIdReference, out ConstructionData construction)) return null;
                    constructionReference = construction;
                }
                return constructionReference;
            }
        }
        [System.NonSerialized] private ConstructionData constructionReference = null;

        /// <summary>
        /// If there's no attached construction reference, than returns null. <br></br>
        /// </summary>
        /// <exception cref="System.NullReferenceException"></exception>
        public TaskResult Result
        {
            get
            {
                if (blueprintBaseIdReference < 0) return null;
                if (result == null)
                {
                    if (ConstructionReference == null) return null;
                    result = new(this, ConstructionReference);
                }
                return result;
            }
        }
        [System.NonSerialized] private TaskResult result = null;
        public bool IsAccepted => BlueprintBaseIdReference > -1;
        #endregion fields & properties

        #region methods
        public void Clarify()
        {
            if (isClarified) return;
            isClarified = true;
            OnClarified?.Invoke();
        }
        internal void ResetBlueprintReference()
        {
            blueprintBaseIdReference = -1;
        }
        internal bool TryAccept(int blueprintBaseReferenceId)
        {
            if (blueprintBaseReferenceId < 0) return false;
            if (blueprintBaseIdReference > -1) return false;
            this.blueprintBaseIdReference = blueprintBaseReferenceId;
            return true;
        }
        protected override ConstructionTaskInfo GetInfo()
        {
            return DB.Instance.ConstructionTaskInfo[Id].Data;
        }
        public ConstructionTaskData(int id) : base(id) { }
        public ConstructionTaskData(int id, int duration, int currentMonth) : base(id, duration, currentMonth) { }
        #endregion methods

        public sealed class TaskResult
        {
            public static readonly float AllowedRoomsAreaDivergence = 0.15f;
            public static readonly float AllowedRoomsCountDivergence = 0.1f;
            public static readonly float AllowedWindowsCountDivergence = 0.3f;
            public static readonly float AllowedResourcesGroupsDivergence = 0.1f;
            public static readonly float AllowedResourcesGroupTypeDivergence = 0.1f;

            /// <summary>
            /// -1..1 but probably it's not reachable due to economic <br></br>
            /// Based on task reward
            /// </summary>
            public float MoneyChangeDivergence { get; }
            public int MoneyChange { get; }
            public int MoodChange { get; }
            public int RatingChange { get; }
            public Description ResultDescription { get; }

            private void CalculateRoomsAreaDivergence(int taskMoneyReward, out int moneyChange)
            {
                float totalDivergence = 0f;
                foreach (RoomDivergence roomDivergence in ResultDescription.RoomsAreaDivergence)
                {
                    totalDivergence += roomDivergence.Divergence;
                }
                if (ResultDescription.RoomsAreaDivergence.Count > 0)
                    totalDivergence /= ResultDescription.RoomsAreaDivergence.Count;
                CalculateDivergenceReward(taskMoneyReward, totalDivergence, AllowedRoomsAreaDivergence, 5, 1.5f, out moneyChange);
            }
            private void CalculateRoomsCountDivergence(int taskMoneyReward, out int moneyChange)
            {
                float totalDivergence = ResultDescription.RoomsCountDivergence;
                CalculateDivergenceReward(taskMoneyReward, totalDivergence, AllowedRoomsCountDivergence, 20, 2, out moneyChange);
            }
            private void CalculateWindowsDivergence(ConstructionTaskData task, int taskMoneyReward, out int moneyChange)
            {
                if (task.Info.WindowsRequired < 2)
                {
                    moneyChange = 0;
                    return;
                }
                float totalDivergence = ResultDescription.WindowsDivergence;
                CalculateDivergenceReward(taskMoneyReward, totalDivergence, AllowedWindowsCountDivergence, 12, 2, out moneyChange);
            }
            private void CalculateResourcesGroupDivergence(ConstructionTaskData task, int taskMoneyReward, out int moneyChange)
            {
                if (!task.Info.UseSingleResourcesGroup)
                {
                    moneyChange = 0;
                    return;
                }
                float totalDivergence = ResultDescription.SingleGroupDivergence;
                CalculateDivergenceReward(taskMoneyReward, totalDivergence, AllowedResourcesGroupsDivergence, 14, 2, out moneyChange);
            }
            private void CalculateResourcesGroupTypeDivergence(ConstructionTaskData task, int taskMoneyReward, out int moneyChange)
            {
                if (task.Info.ResourcesType == 0)
                {
                    moneyChange = 0;
                    return;
                }
                float totalDivergence = ResultDescription.GroupTypeDivergence;
                CalculateDivergenceReward(taskMoneyReward, totalDivergence, AllowedResourcesGroupTypeDivergence, 9, 1.5f, out moneyChange);
            }
            private void CalculateDivergenceReward(int taskMoneyReward, float totalDivergence, float maxPositiveDivergence, float positiveMoneyDivide, float negativeMoneyDivide, out int moneyChange)
            {
                if (totalDivergence - maxPositiveDivergence <= 0)
                {
                    float lerp = Mathf.Clamp01(totalDivergence / maxPositiveDivergence);
                    moneyChange = (int)((taskMoneyReward / positiveMoneyDivide) * (1 - lerp));
                    return;
                }
                float negativeLerp = Mathf.Clamp01(totalDivergence / (1 - maxPositiveDivergence));
                negativeLerp = Mathf.Clamp01(Mathf.Pow(1 + negativeLerp, 1.2f) - 1);
                moneyChange = -(int)((taskMoneyReward / negativeMoneyDivide) * negativeLerp);
            }

            public TaskResult(ConstructionTaskData task, ConstructionData completedConstruction)
            {
                ResultDescription = new(task, completedConstruction);
                task.Info.RewardInfo.Rewards.Exists(x => x.Type == RewardType.Money, out Reward moneyReward);
                int taskMoneyReward = moneyReward.Value;
                CalculateRoomsAreaDivergence(taskMoneyReward, out int roomsMoneyChange);
                CalculateRoomsCountDivergence(taskMoneyReward, out int roomsCountMoneyChange);
                CalculateWindowsDivergence(task, taskMoneyReward, out int windowsMoneyChange);
                CalculateResourcesGroupDivergence(task, taskMoneyReward, out int resourcesGroupMoneyChange);
                CalculateResourcesGroupTypeDivergence(task, taskMoneyReward, out int resourcesGroupTypeMoneyChange);

                MoneyChange = roomsMoneyChange + roomsCountMoneyChange + windowsMoneyChange + resourcesGroupMoneyChange + resourcesGroupTypeMoneyChange;

                int maxMoodChange = 5;
                MoneyChangeDivergence = Mathf.Clamp(MoneyChange, -taskMoneyReward, taskMoneyReward) / (float)taskMoneyReward;
                MoodChange = (int)(maxMoodChange * (MoneyChangeDivergence * 2));

                int maxPositiveRatingChange = 2;
                int maxNegativeRatingChange = 3;
                if (!task.Info.RewardInfo.Rewards.Exists(x => x.Type == RewardType.Rating, out Reward ratingReward))
                {
                    maxPositiveRatingChange = 3;
                    maxNegativeRatingChange = 4;
                }

                if (MoneyChangeDivergence >= 0)
                    RatingChange = Mathf.FloorToInt(MoneyChangeDivergence * maxPositiveRatingChange);
                else
                    RatingChange = Mathf.CeilToInt(MoneyChangeDivergence * maxNegativeRatingChange);

                if (task.Info.MaxRating > 0 && task.Info.MaxRating > GameData.Data.CompanyData.Rating.Value)
                {
                    RatingChange = Mathf.Min(RatingChange, 0);
                }
            }

            public sealed class Description
            {
                public IReadOnlyList<RoomDivergence> RoomsAreaDivergence => roomsAreaDivergence;
                private List<RoomDivergence> roomsAreaDivergence = new();
                /// <summary>
                /// 0..1; 0 = OK; 1 = BAD
                /// </summary>
                public float RoomsCountDivergence => roomsCountDivergence;
                private float roomsCountDivergence;
                /// <summary>
                /// 0..1; 0 = OK; 1 = BAD
                /// </summary>
                public float WindowsDivergence => windowsDivergence;
                private float windowsDivergence;
                /// <summary>
                /// 0..1; 0 = OK; 1 = BAD
                /// </summary>
                public float GroupTypeDivergence => groupTypeDivergence;
                private float groupTypeDivergence;
                /// <summary>
                /// 0..1; 0 = OK; 1 = BAD
                /// </summary>
                public float SingleGroupDivergence => singleGroupDivergence;
                private float singleGroupDivergence;

                private void CalculateRoomsDivergence(ConstructionData completedConstruction)
                {
                    roomsAreaDivergence.Clear();
                    completedConstruction.CalculateRoomDivergence(roomsAreaDivergence, out roomsCountDivergence);
                }
                private void CalculateResourcesDivergence(ConstructionTaskInfo taskInfo, ConstructionData completedConstruction)
                {
                    int windowsCount = 0;
                    CountableItemList<ConstructionResourceGroup> groupsCount = new();
                    foreach (BlueprintResourceData resource in completedConstruction.BlueprintResources)
                    {
                        ConstructionResourceInfo info = resource.ResourceInfo;
                        ConstructionSubtype resSubtype = info.ConstructionSubtype;
                        if (resSubtype == ConstructionSubtype.Window)
                        {
                            windowsCount++;
                        }
                        ConstructionResourceGroup group = info.RelatedGroup;
                        if (group == null) continue;
                        int id = group.Id;
                        groupsCount.AddItem(group, x => x.Item.Id == id);
                    }
                    if (taskInfo.WindowsRequired > 0)
                        windowsDivergence = 1f - Mathf.Clamp01(windowsCount / (float)taskInfo.WindowsRequired);
                    else
                        windowsDivergence = 0f;

                    int totalGroupElements = 0;
                    int maxGroupCount = 0;
                    int totalNonUnknownGroupElements = 0;
                    int totalRequiredGroupElements = 0;
                    ConstructionResourceGroupType requiredGroup = taskInfo.ResourcesType;
                    foreach (var el in groupsCount.Items)
                    {
                        int count = el.Count;
                        totalGroupElements += count;
                        if (count > maxGroupCount)
                            maxGroupCount = count;

                        if (el.Item.GroupTypes > 0)
                        {
                            totalNonUnknownGroupElements += count;
                        }
                        if (el.Item.GroupTypes.HasFlag(requiredGroup))
                        {
                            totalRequiredGroupElements += count;
                        }
                    }
                    if (taskInfo.UseSingleResourcesGroup)
                        singleGroupDivergence = 1f - Mathf.Clamp01(maxGroupCount / (float)totalGroupElements);
                    else
                        singleGroupDivergence = 0f;

                    if (requiredGroup == 0)
                        groupTypeDivergence = 0f;
                    else
                        groupTypeDivergence = 1f - Mathf.Clamp01(totalRequiredGroupElements / (float)totalNonUnknownGroupElements);
                }
                public Description(ConstructionTaskData task, ConstructionData completedConstruction)
                {
                    ConstructionTaskInfo taskInfo = task.Info;
                    CalculateRoomsDivergence(completedConstruction);
                    CalculateResourcesDivergence(taskInfo, completedConstruction);
                }

            }

        }
    }
}