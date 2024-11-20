using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Universal.Core;
using static Game.Serialization.World.ConstructionTaskData;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ReviewData
    {
        #region fields & properties
        private static readonly int[] VeryBadWorkTexts = new int[] { 220, 221, 222, 223, 224, 225 };
        private static readonly int[] ExpiredTexts = new int[] { 226, 227, 228, 229, 230 };
        private static readonly int[] NoCommentTexts = new int[] { 231, 232, 233, 234, 235, 236 };

        private static readonly int[] WrongRoomsCountTexts = new int[] { 237, 238, 239 };
        private static readonly int[] WrongRoomsAreaTexts = new int[] { 240, 241, 242, 243 };
        private static readonly int[] WrongWindowsCountTexts = new int[] { 244, 245, 246, 247 };
        private static readonly int[] WrongGroupTexts = new int[] { 399, 400, 401, 402, 403 };
        private static readonly int[] WrongGroupTypeTexts = new int[] { 404, 405, 406, 407 };

        private static readonly int[] BadWorkTexts = new int[] { 248, 249, 250, 251, 252 };
        private static readonly int[] NeutralWorkTexts = new int[] { 253, 254, 255, 256, 257 };
        private static readonly int[] GoodWorkTexts = new int[] { 258, 259, 260, 261, 262, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429 };

        [SerializeField][Min(0)] private int taskId = 0;
        public IReadOnlyList<int> GameTextIds => gameTextIds;
        [SerializeField] private List<int> gameTextIds = new();
        public bool IsPositive => isPositive;
        [SerializeField] private bool isPositive = true;
        public int Rating => rating;
        [SerializeField][Min(0)] private int rating = 0;
        public int MonthCreated => monthCreated;
        [SerializeField] private int monthCreated = 0;
        public ConstructionTaskData Task
        {
            get
            {
                if (task == null)
                {
                    LoadDataFromTask();
                }
                return task;
            }
        }
        [System.NonSerialized] private ConstructionTaskData task = null;
        public bool IsTaskCompleted
        {
            get
            {
                if (task == null)
                    LoadDataFromTask();
                return isTaskCompleted;
            }
        }
        [System.NonSerialized] private bool isTaskCompleted = false;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Runtime only
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (gameTextIds.Count == 0)
                UpdateData();
            StringBuilder sb = new();
            int count = gameTextIds.Count;
            for (int i = 0; i < count; ++i)
            {
                sb.Append($"{LanguageInfo.GetTextByType(TextType.Game, gameTextIds[i])} ");
            }
            return sb.ToString();
        }
        private void UpdateData()
        {
            GenerateText();
            UpdateStatus();
            UpdateRating();
            UpdateMonth();
        }
        private void GenerateText()
        {
            gameTextIds.Clear();
            if (!IsTaskCompleted)
            {
                GenerateTextForExpiredTask();
                return;
            }
            GenerateTextForCompletedTask();
        }
        private void UpdateStatus()
        {
            if (!IsTaskCompleted)
            {
                isPositive = false;
                return;
            }
            if (IsTextContainsAnyId(GoodWorkTexts) || IsTextContainsAnyId(NeutralWorkTexts) || IsTextContainsAnyId(NoCommentTexts))
            {
                isPositive = true;
                return;
            }
            isPositive = false;
        }
        private void UpdateRating()
        {
            int companyRating = GameData.Data.CompanyData.Rating.Value;
            if (isPositive)
            {
                rating = Mathf.Clamp(companyRating + Random.Range(0, 10), 0, 100);
                return;
            }
            if (IsTextContainsAnyId(ExpiredTexts))
            {
                rating = 0;
                return;
            }
            if (IsTextContainsAnyId(BadWorkTexts))
            {
                rating = Mathf.Clamp(Mathf.FloorToInt(companyRating / Random.Range(1.3f, 3)), 0, 100);
                return;
            }
            if (IsTextContainsAnyId(VeryBadWorkTexts))
            {
                rating = CustomMath.GetRandomChance(40) ? 0 : Mathf.Clamp(Mathf.FloorToInt(companyRating / Random.Range(5, 20)), 0, 100);
                return;
            }
        }
        private void UpdateMonth()
        {
            monthCreated = GameData.Data.PlayerData.MonthData.CurrentMonth;
        }
        private bool IsTextContainsAnyId(int[] ids)
        {
            for (int i = 0; i < ids.Length; ++i)
            {
                if (gameTextIds.Contains(ids[i])) return true;
            }
            return false;
        }
        private void GenerateTextForExpiredTask()
        {
            int textCount = Random.Range(0, 3);
            if (textCount == 0)
            {
                AddRandomizedId(NoCommentTexts);
                return;
            }

            bool showBadTextFirst = CustomMath.GetRandomChance(50);
            if (showBadTextFirst)
                AddRandomizedId(VeryBadWorkTexts);
            else
                AddRandomizedId(ExpiredTexts);

            if (textCount == 1) return;

            if (showBadTextFirst)
                AddRandomizedId(ExpiredTexts);
            else
                AddRandomizedId(VeryBadWorkTexts);
        }
        private void GenerateTextForCompletedTask()
        {
            TaskResult result = Task.Result;
            TaskResult.Description description = result.ResultDescription;

            AddCompletedDivergenceText(description);
            AddCompletedHeaderText(result);

            if (gameTextIds.Count == 0)
            {
                AddRandomizedId(NoCommentTexts);
            }
        }
        private void AddCompletedHeaderText(TaskResult result)
        {
            if (CustomMath.GetRandomChance(70))
                AddTotalRatingText(result);
        }
        private void AddCompletedDivergenceText(TaskResult.Description description)
        {
            List<int> order = GetRandomCompletedTextOrder();
            int count = order.Count;
            for (int i = 0; i < count; ++i)
            {
                switch (order[i])
                {
                    case 0: AddRoomsCountDivergenceText(description); break;
                    case 1: AddRoomsAreaDivergenceText(description); break;
                    case 2: AddWindowsCountDivergenceText(description); break;
                    case 3: AddGroupDivergenceText(description); break;
                    case 4: AddGroupTypeDivergenceText(description); break;
                }
            }
        }
        private List<int> GetRandomCompletedTextOrder()
        {
            List<int> order = new() { 0, 1, 2, 3, 4 };
            order.Shuffle();
            return order;
        }
        private void AddTotalRatingText(TaskResult result)
        {
            float moneyDivergence = result.MoneyChangeDivergence;
            switch (moneyDivergence)
            {
                case float i when i < -0.3f: AddRandomizedId(VeryBadWorkTexts); break;
                case float i when i < -0.1f: AddRandomizedId(BadWorkTexts); break;
                case float i when i >= -0.1f && i <= 0.1f: AddRandomizedId(NeutralWorkTexts); break;
                case float i when i > 0.1f: AddRandomizedId(GoodWorkTexts); break;
                default: break;
            }
        }
        private void AddRoomsCountDivergenceText(TaskResult.Description description)
        {
            float roomsCountDivergence = description.RoomsCountDivergence;
            bool isGoodDivergence = roomsCountDivergence < TaskResult.AllowedRoomsCountDivergence;
            if (isGoodDivergence)
            {

            }
            else
            {
                if (CustomMath.GetRandomChance(50))
                    AddRandomizedId(WrongRoomsCountTexts);
            }
        }
        private void AddRoomsAreaDivergenceText(TaskResult.Description description)
        {
            float avgAreaDivergence = 0f;
            int count = description.RoomsAreaDivergence.Count;

            for (int i = 0; i < count; ++i)
            {
                var divergence = description.RoomsAreaDivergence[i];
                avgAreaDivergence += divergence.Divergence;
            }
            avgAreaDivergence /= count;

            bool isGoodDivergence = avgAreaDivergence < TaskResult.AllowedRoomsAreaDivergence;
            if (isGoodDivergence)
            {

            }
            else
            {
                if (CustomMath.GetRandomChance(70))
                    AddRandomizedId(WrongRoomsAreaTexts);
            }
        }
        private void AddWindowsCountDivergenceText(TaskResult.Description description)
        {
            float windowsDivergence = description.WindowsDivergence;
            bool isGoodDivergence = windowsDivergence < TaskResult.AllowedWindowsCountDivergence;
            if (isGoodDivergence)
            {

            }
            else
            {
                if (CustomMath.GetRandomChance(50))
                    AddRandomizedId(WrongWindowsCountTexts);
            }
        }
        private void AddGroupDivergenceText(TaskResult.Description description)
        {
            float divergence = description.SingleGroupDivergence;
            bool isGoodDivergence = divergence < TaskResult.AllowedResourcesGroupsDivergence;
            if (isGoodDivergence)
            {

            }
            else
            {
                if (CustomMath.GetRandomChance(50))
                    AddRandomizedId(WrongGroupTexts);
            }
        }
        private void AddGroupTypeDivergenceText(TaskResult.Description description)
        {
            float divergence = description.GroupTypeDivergence;
            bool isGoodDivergence = divergence < TaskResult.AllowedResourcesGroupTypeDivergence;
            if (isGoodDivergence)
            {

            }
            else
            {
                if (CustomMath.GetRandomChance(50))
                    AddRandomizedId(WrongGroupTypeTexts);
            }
        }
        private void AddRandomizedId(int[] array) => gameTextIds.Add(RandomizeId(array));
        private int RandomizeId(int[] array) => array[Random.Range(0, array.Length)];
        private void LoadDataFromTask()
        {
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            if (tasks.CompletedTasks.Exists(x => x.Id == taskId, out task))
            {
                isTaskCompleted = true;
                return;
            }
            if (tasks.ExpiredTasks.Exists(x => x.Id == taskId, out task))
            {
                isTaskCompleted = false;
                return;
            }
        }
        /// <summary>
        /// Lazy creation
        /// </summary>
        /// <param name="taskId"></param>
        public ReviewData(int taskId)
        {
            this.taskId = taskId;
        }
        /// <summary>
        /// Runtime only. Non-lazy
        /// </summary>
        /// <param name="task"></param>
        public ReviewData(ConstructionTaskData task)
        {
            this.taskId = task.Id;
            LoadDataFromTask();
            UpdateData();
        }
        #endregion methods
    }
}