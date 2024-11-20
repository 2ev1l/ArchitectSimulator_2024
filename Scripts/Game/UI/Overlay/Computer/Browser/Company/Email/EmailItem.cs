using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class EmailItem : ContextActionsItem<ConstructionTaskData>
    {
        #region fields & properties
        [SerializeField] private HumanItem humanItem;
        [SerializeField] private TextMeshProUGUI languageDescriptionText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI timeLeftText;
        [SerializeField] private TextMeshProUGUI clarifyingPriceText;
        [SerializeField] private CustomButton rejectButton;
        [SerializeField] private CustomButton clarifyButton;
        [SerializeField] private GameObject notAcceptedTextObject;
        [SerializeField] private GameObject clarifyingPriceObject;
        [SerializeField] private MinimalRatingExposer ratingExposer;
        private static readonly LanguageInfo ClaryfyingInfo = new(269, TextType.Game);
        private static readonly LanguageInfo PlaceholderDescriptionInfo = new(364, TextType.Game);
        private static readonly LanguageInfo RewardsLanguage = new(96, TextType.Game);
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            rejectButton.OnClicked += RejectTask;
            clarifyButton.OnClicked += ClarifyTask;
            GameData.Data.PlayerData.Wallet.OnValueChanged += UpdateClarificationObject;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            rejectButton.OnClicked -= RejectTask;
            clarifyButton.OnClicked -= ClarifyTask;
            GameData.Data.PlayerData.Wallet.OnValueChanged -= UpdateClarificationObject;
        }
        private void ClarifyTask()
        {
            Context.Clarify();
            GameData.Data.PlayerData.Wallet.TryDecreaseValue(Context.Info.ClarificationPrice);
            UpdateClarificationObject();
            UpdateDescription();
        }
        private void RejectTask()
        {
            if (Context.BlueprintBaseIdReference > -1)
            {
                InfoRequest.GetErrorRequest(401).Send();
                return;
            }
            GameData.Data.CompanyData.ConstructionTasks.RejectTask(Context);
        }
        private void UpdateClarificationObject(int _1, int _2) => UpdateClarificationObject();
        private void UpdateClarificationObject()
        {
            if (!Context.IsClarified && Context.Info.CanClarifyDetails)
            {
                if (!clarifyingPriceObject.activeSelf)
                    clarifyingPriceObject.SetActive(true);
                clarifyButton.enabled = GameData.Data.PlayerData.Wallet.CanDecreaseValue(Context.Info.ClarificationPrice);
            }
            else
            {
                if (clarifyingPriceObject.activeSelf)
                    clarifyingPriceObject.SetActive(false);
            }
        }
        private void UpdateDescription()
        {
            StringBuilder sb = new();
            string desc = Context.Info.DescriptionInfo.Text;

            if (desc.Length <= 0)
            {
                desc = PlaceholderDescriptionInfo.Text;
                languageDescriptionText.text = PlaceholderDescriptionInfo.Text;
            }
            string rewards = Context.Info.RewardInfo.GetLanguage();
            int rewardsSize = 90;
            if (rewards.Length > 0)
                languageDescriptionText.text = $"{desc}<align=center><size={rewardsSize}%>\n\n{RewardsLanguage.Text}:\n{rewards}</size></align>";
            else
                languageDescriptionText.text = desc;

            int claryfyingInfoSize = 100;
            int claryfyingDescriptionSize = 90;
            BlueprintInfo blueprintInfo = Context.Info.BlueprintInfo;
            BuildingInfo buildingInfo = blueprintInfo.BuildingInfo;
            sb.Append($"<size={claryfyingInfoSize}%>{ClaryfyingInfo.Text}\n\n</size>");
            sb.Append($"<size={claryfyingDescriptionSize}%>{buildingInfo.BuildingType.ToLanguage()}-{buildingInfo.BuildingStyle.ToLanguage()}\n");
            sb.Append($"[{BuildingFloor.F1_Flooring.ToLanguage()}..{buildingInfo.MaxFloor.ToLanguage()}]\n");
            int floorRoomsInfoCount = blueprintInfo.RoomsInfo.Count;
            for (int i = 0; i < floorRoomsInfoCount; ++i)
            {
                FloorRoomsInfo floorRoomsInfo = blueprintInfo.RoomsInfo[i];
                sb.Append($"{floorRoomsInfo.Floor.ToLanguage()}\n");
                int roomsInfoCount = floorRoomsInfo.Rooms.Count;
                for (int j = 0; j < roomsInfoCount; ++j)
                {
                    BuildingRoomInfo roomInfo = floorRoomsInfo.Rooms[j];
                    sb.Append($"{roomInfo.ToLanguage()}\n");
                }
            }
            if (Context.IsClarified)
            {
                sb.Append($"\n{Context.Info.GetClarificationText()}");
            }

            descriptionText.text = sb.ToString();
        }
        private void UpdateAcceptableObjects()
        {
            GameObject rejectBtn = rejectButton.gameObject;
            bool isNotAccepted = Context.BlueprintBaseIdReference < 0;
            bool isRepeatable = Context.Info.IsRepeatable;
            bool buttonEnabled = isNotAccepted && isRepeatable;
            if (rejectBtn.activeSelf != buttonEnabled)
            {
                rejectBtn.SetActive(buttonEnabled);
            }
            if (notAcceptedTextObject.activeSelf != isNotAccepted)
            {
                notAcceptedTextObject.SetActive(isNotAccepted);
            }
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            UpdateAcceptableObjects();
            timeLeftText.text = $"{Context.MonthLeft} m.";

            UpdateClarificationObject();
            clarifyingPriceText.text = $"${Context.Info.ClarificationPrice}";

            UpdateDescription();
            ratingExposer.Expose(Context.Info);
        }
        public override void OnListUpdate(ConstructionTaskData param)
        {
            if (Context == param)
            {
                UpdateUI();
                return;
            }
            base.OnListUpdate(param);
            humanItem.OnListUpdate(param.HumanInfo);
        }
        #endregion methods
    }
}