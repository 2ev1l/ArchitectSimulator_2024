using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Universal.Core;
using static Game.Serialization.World.ConstructionTaskData;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class ConstructionTaskResultPanel : MonoBehaviour
    {
        #region fields & properties
        private static readonly LanguageInfo BadCommentaryInfo = new(213, TextType.Game);
        private static readonly LanguageInfo GoodCommentaryInfo = new(214, TextType.Game);
        private static readonly LanguageInfo InsufficientRoomsAreaInfo = new(215, TextType.Game);
        private static readonly LanguageInfo InsufficientRoomsCountInfo = new(216, TextType.Game);
        private static readonly LanguageInfo InsufficientWindowsInfo = new(217, TextType.Game);
        private static readonly LanguageInfo InsufficientGroupInfo = new(397, TextType.Game);
        private static readonly LanguageInfo InsufficientGroupTypeInfo = new(398, TextType.Game);
        private static readonly LanguageInfo GoodWorkInfo = new(218, TextType.Game);
        private static readonly LanguageInfo BadWorkInfo = new(219, TextType.Game);
        private static readonly LanguageInfo AdditionallyInfo = new(395, TextType.Game);

        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI infoText;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.CompanyData.ConstructionTasks.OnTaskCompleted += OnTaskCompleted;
        }
        private void OnDisable()
        {
            GameData.Data.CompanyData.ConstructionTasks.OnTaskCompleted -= OnTaskCompleted;
        }
        private void OnTaskCompleted(ConstructionTaskData task)
        {
            TaskResult result = task.Result;

            TaskResult.Description description = result.ResultDescription;
            StringBuilder sb = new();
            AddBaseRewardsText(sb, task);
            sb.Append($"\n\n{AdditionallyInfo.Text}:");
            AddAdditionalRewardsText(sb, result);
            string desc = GetDescriptionText(description);
            if (desc.Length > 0)
            {
                sb.Append($"\n\n<style=DecreaseLight>{BadCommentaryInfo.Text}</style>");
                sb.Append($"{desc}");
            }
            else
            {
                sb.Append($"\n\n<style=IncreaseLight>{GoodCommentaryInfo.Text}</style><br>");
            }

            if (result.MoneyChange > 0)
                sb.Append($"\n<style=IncreaseLight>{GoodWorkInfo.Text}</style>");
            else
                sb.Append($"\n<style=DecreaseDark>{BadWorkInfo.Text}</style>");

            infoText.text = sb.ToString();
            panel.SetActive(true);
        }
        private string GetDescriptionText(TaskResult.Description description)
        {
            StringBuilder sb = new();
            if (description.RoomsCountDivergence > TaskResult.AllowedRoomsCountDivergence)
            {
                sb.Append($"\n{InsufficientRoomsCountInfo.Text}<br>");
            }
            string badRoomsAreaText = GetBadRoomsAreaText(description);
            if (badRoomsAreaText.Length > 0)
            {
                sb.Append($"\n{InsufficientRoomsAreaInfo.Text}");
                sb.Append($"{badRoomsAreaText}<br>");
            }
            if (description.WindowsDivergence > TaskResult.AllowedWindowsCountDivergence)
            {
                sb.Append($"\n{InsufficientWindowsInfo.Text}<br>");
            }
            if (description.SingleGroupDivergence > TaskResult.AllowedResourcesGroupsDivergence)
            {
                sb.Append($"\n{InsufficientGroupInfo.Text}<br>");
            }
            if (description.GroupTypeDivergence > TaskResult.AllowedResourcesGroupTypeDivergence)
            {
                sb.Append($"\n{InsufficientGroupTypeInfo.Text}<br>");
            }
            return sb.ToString();
        }
        private string GetBadRoomsAreaText(TaskResult.Description description)
        {
            StringBuilder sb = new();
            foreach (var roomDivergence in description.RoomsAreaDivergence)
            {
                if (roomDivergence.Divergence < TaskResult.AllowedRoomsAreaDivergence) continue;
                sb.Append($"\n{roomDivergence.Floor.ToLanguage()} : {roomDivergence.Room.ToLanguage()} : ~{roomDivergence.MaxOccupiedArea:F2} m2 / {roomDivergence.TargetArea:F2} m2");
            }
            return sb.ToString();
        }
        private void AddBaseRewardsText(StringBuilder sb, ConstructionTaskData task)
        {
            RewardInfo info = task.Info.RewardInfo;
            if (info.HasReward(RewardType.Money, out Reward moneyReward)) AddValueLine(sb, moneyReward.Value, "Money");
            if (info.HasReward(RewardType.Mood, out Reward moodReward)) AddValueLine(sb, moodReward.Value, "Mood");
            if (info.HasReward(RewardType.Rating, out Reward ratingReward)) AddValueLine(sb, ratingReward.Value, "Rating");
        }
        private void AddAdditionalRewardsText(StringBuilder sb, TaskResult result)
        {
            AddValueLine(sb, result.MoneyChange, "Money");
            AddValueLine(sb, result.MoodChange, "Mood");
            AddValueLine(sb, result.RatingChange, "Rating");
        }
        private void AddValueLine(StringBuilder sb, int value, string styleName)
        {
            if (value > 0)
            {
                sb.Append($"\n<style=\"IncreaseLight\">+<style={styleName}>{value}</style></style>");
                return;
            }
            if (value < 0)
            {
                sb.Append($"\n<style=\"DecreaseLight\">-<style={styleName}>{Mathf.Abs(value)}</style></style>");
                return;
            }
        }
        private string InsertColorTag(string text, string color) => $"<color=#{color}>{text}</color>";
        #endregion methods
    }
}