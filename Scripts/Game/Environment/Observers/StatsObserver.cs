using EditorCustom.Attributes;
using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Universal.Time;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public class StatsObserver : Observer
    {
        #region fields & properties
        [SerializeField] private ChangedAmountPopupRequest moneyPopup;
        [SerializeField] private ChangedAmountPopupRequest ratingPopup;
        [SerializeField] private ChangedAmountPopupRequest moodPopup;
        [SerializeField] private ChangedAmountPopupRequest timePopup;

        [SerializeField] private StaticTextPopupRequest newTaskPopup;
        [SerializeField] private StaticTextPopupRequest completedTaskPopup;
        [SerializeField] private StaticTextPopupRequest expiredTaskPopup;

        [SerializeField] private StaticTextPopupRequest reviewPopup;
        [SerializeField] private StaticTextPopupRequest collectiblePopup;
        [SerializeField] private StaticTextPopupRequest newLandPlotOfferPopup;

        private static readonly LanguageInfo newTaskInfo = new(0, TextType.Task);
        private static readonly LanguageInfo completedTaskInfo = new(1, TextType.Task);
        private static readonly LanguageInfo expiredTaskInfo = new(127, TextType.Task);
        private static readonly LanguageInfo newReviewInfo = new(263, TextType.Game);
        private static readonly LanguageInfo newCollectibleInfo = new(361, TextType.Game);
        private static readonly LanguageInfo newItemsPerRatingInfo = new(408, TextType.Game);
        private static readonly LanguageInfo newLandPlotOfferInfo = new(430, TextType.Game);

        private TimeDelay newLandPlotOfferPopupDelay = new(0.1f);
        #endregion fields & properties

        #region methods
        private void SendCollectiblePopupRequest(int _) => SendCollectiblePopupRequest();
        private void SendCollectiblePopupRequest()
        {
            SendStaticTextPopupRequest(collectiblePopup, $"{newCollectibleInfo.Text} {GameData.Data.EnvironmentData.Collectibles.Items.Count}/{EnvironmentData.TOTAL_COLLECTIBLES}");
        }
        private void SendTaskExpiredPopupRequest(ConstructionTaskData _) => SendTaskExpiredPopupRequest();
        private void SendTaskExpiredPopupRequest()
        {
            SendStaticTextPopupRequest(expiredTaskPopup, expiredTaskInfo.Text);
        }
        private void SendReviewPopupRequest(ReviewData _) => SendReviewPopupRequest();
        private void SendReviewPopupRequest()
        {
            SendStaticTextPopupRequest(reviewPopup, newReviewInfo.Text);
        }
        private void SendTaskStartPopupRequest(PlayerTaskData _) => SendTaskStartPopupRequest();
        public void SendTaskStartPopupRequest()
        {
            SendStaticTextPopupRequest(newTaskPopup, newTaskInfo.Text);
        }
        private void SendTaskCompletedPopupRequest(PlayerTaskData _) => SendTaskCompletedPopupRequest();
        public void SendTaskCompletedPopupRequest()
        {
            SendStaticTextPopupRequest(completedTaskPopup, completedTaskInfo.Text);
        }
        public void SendNewLandPlotOfferPopupRequest(LandPlotOfferData _) => SendNewLandPlotOfferPopupRequest();
        public void SendNewLandPlotOfferPopupRequest()
        {
            if (!newLandPlotOfferPopupDelay.CanActivate) return;
            SendStaticTextPopupRequest(newLandPlotOfferPopup, newLandPlotOfferInfo.Text);
            newLandPlotOfferPopupDelay.Activate();
        }
        private void SendStaticTextPopupRequest(StaticTextPopupRequest staticTextPopupRequest, string text)
        {
            staticTextPopupRequest.Text = text;
            staticTextPopupRequest.Send();
        }
        public void SendMoneyPopupRequest(int currentValue, int changedAmount)
        {
            SendChangedAmountPopup(moneyPopup, currentValue, changedAmount);
        }
        public void SendRatingPopupRequest(int currentValue, int changedAmount)
        {
            SendChangedAmountPopup(ratingPopup, currentValue, changedAmount);
        }
        public void SendMoodPopupRequest(int currentValue, int changedAmount)
        {
            SendChangedAmountPopup(moodPopup, currentValue, changedAmount);
        }
        public void SendTimePopupRequest(int currentValue, int changedAmount)
        {
            SendChangedAmountPopup(timePopup, currentValue, changedAmount);
        }
        private void SendChangedAmountPopup(ChangedAmountPopupRequest popupRequest, int totalAmount, int changedAmount)
        {
            popupRequest.ChangedAmount = changedAmount;
            popupRequest.TotalAmount = totalAmount;
            popupRequest.Send();
        }
        public void SendRatingObjectsUnlockPopup(int currentValue, int changedAmount)
        {
            if (changedAmount < 0) return;
            var minRatingHandlers = DB.Instance.GetMinRatingHandlers();
            foreach (var el in minRatingHandlers)
            {
                if (el.MinRating == currentValue)
                {
                    new ImportantInfoRequest(newItemsPerRatingInfo.Text).Send();
                    return;
                }
            }
        }

        public override void Initialize()
        {
            PlayerData playerData = GameData.Data.PlayerData;
            playerData.Wallet.OnValueChanged += SendMoneyPopupRequest;
            playerData.Mood.OnValueChanged += SendMoodPopupRequest;
            playerData.MonthData.FreeTime.OnValueChanged += SendTimePopupRequest;

            playerData.Tasks.OnTaskCompleted += SendTaskCompletedPopupRequest;
            playerData.Tasks.OnTaskStarted += SendTaskStartPopupRequest;

            CompanyData companyData = GameData.Data.CompanyData;
            companyData.Rating.OnValueChanged += SendRatingPopupRequest;
            companyData.Rating.OnValueChanged += SendRatingObjectsUnlockPopup;
            companyData.ReviewsData.OnReviewAdded += SendReviewPopupRequest;
            companyData.ConstructionTasks.OnTaskExpired += SendTaskExpiredPopupRequest;
            companyData.LandPlotsData.OnOfferAdded += SendNewLandPlotOfferPopupRequest;

            EnvironmentData environmentData = GameData.Data.EnvironmentData;
            environmentData.Collectibles.OnItemAdded += SendCollectiblePopupRequest;
        }
        public override void Dispose()
        {
            PlayerData playerData = GameData.Data.PlayerData;
            playerData.Wallet.OnValueChanged -= SendMoneyPopupRequest;
            playerData.Mood.OnValueChanged -= SendMoodPopupRequest;
            playerData.MonthData.FreeTime.OnValueChanged -= SendTimePopupRequest;

            playerData.Tasks.OnTaskCompleted -= SendTaskCompletedPopupRequest;
            playerData.Tasks.OnTaskStarted -= SendTaskStartPopupRequest;

            CompanyData companyData = GameData.Data.CompanyData;
            companyData.Rating.OnValueChanged -= SendRatingPopupRequest;
            companyData.Rating.OnValueChanged -= SendRatingObjectsUnlockPopup;
            companyData.ReviewsData.OnReviewAdded -= SendReviewPopupRequest;
            companyData.ConstructionTasks.OnTaskExpired -= SendTaskExpiredPopupRequest;
            companyData.LandPlotsData.OnOfferAdded -= SendNewLandPlotOfferPopupRequest;

            EnvironmentData environmentData = GameData.Data.EnvironmentData;
            environmentData.Collectibles.OnItemAdded -= SendCollectiblePopupRequest;
        }
        #endregion methods
    }
}