using DebugStuff;
using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Universal.Core;
using Universal.Behaviour;
using Game.Serialization.World;
using Game.Events;
using Game.DataBase;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public class AchievementsObserver : Observer
    {
        #region fields & properties
        [SerializeField] private GameOverChecker gameOverChecker;
        #endregion fields & properties

        #region methods
        public override void Initialize()
        {
            CompanyData company = GameData.Data.CompanyData;
            PlayerData player = GameData.Data.PlayerData;
            OfficeData office = company.OfficeData;

            company.Rating.OnValueChanged += OnRatingChanged;
            player.Wallet.OnValueChanged += OnMoneyChanged;
            GameData.Data.EnvironmentData.Collectibles.OnItemAdded += OnCollectibleFound;
            GameData.Data.ConstructionsData.OnConstructionBuilded += OnConstructionBuilded;
            office.OnEmployeeHired += OnEmployeeHired;
            gameOverChecker.OnEndReasonGot += OnGameOver;
            company.ConstructionTasks.OnTaskAccepted += OnConstructionTaskAccepted;
            company.ReviewsData.OnReviewAdded += OnReviewAdded;
            company.LandPlotsData.OnPlotSold += OnLandPlotSold;
            company.WarehouseData.OnInfoChanged += OnWarehouseChanged;
            _ = SteamManager.Initialized;
        }
        public override void Dispose()
        {
            CompanyData company = GameData.Data.CompanyData;
            PlayerData player = GameData.Data.PlayerData;
            OfficeData office = company.OfficeData;

            company.Rating.OnValueChanged -= OnRatingChanged;
            player.Wallet.OnValueChanged -= OnMoneyChanged;
            GameData.Data.EnvironmentData.Collectibles.OnItemAdded -= OnCollectibleFound;
            GameData.Data.ConstructionsData.OnConstructionBuilded -= OnConstructionBuilded;
            office.OnEmployeeHired -= OnEmployeeHired;
            gameOverChecker.OnEndReasonGot -= OnGameOver;
            company.ConstructionTasks.OnTaskAccepted -= OnConstructionTaskAccepted;
            company.ReviewsData.OnReviewAdded -= OnReviewAdded;
            company.LandPlotsData.OnPlotSold -= OnLandPlotSold;
            company.WarehouseData.OnInfoChanged -= OnWarehouseChanged;
        }
        private void OnWarehouseChanged()
        {
            if (GameData.Data.CompanyData.WarehouseData.Id == 15)
            {
                SetAchievement("ACH_W_MAX");
            }
        }
        private void OnLandPlotSold(LandPlotData soldPlot)
        {
            if (GameData.Data.CompanyData.LandPlotsData.SoldPlots.Count >= 10)
            {
                SetAchievement("ACH_LP_10");
            }
        }
        private void OnReviewAdded(ReviewData review)
        {
            if (GameData.Data.CompanyData.ReviewsData.Reviews.Count >= 100)
            {
                SetAchievement("ACH_RV_100");
            }
        }
        private void OnGameOver(GameOverChecker.EndReason endReason)
        {
            switch (endReason)
            {
                case GameOverChecker.BillsReason: SetAchievement("ACH_END_BILL"); break;
                case GameOverChecker.MoodReason: SetAchievement("ACH_END_MOOD"); break;
                case GameOverChecker.AgeReason: SetAchievement("ACH_END_AGE"); break;
                case GameOverChecker.SaturationReason: SetAchievement("ACH_END_STARVE"); break;
            }
        }
        private void OnEmployeeHired(EmployeeData _)
        {
            DivisionsData divisions = GameData.Data.CompanyData.OfficeData.Divisions;
            bool hasBuilders = divisions.Builders.Employees.Count > 0;
            bool hasHr = divisions.HRManager.IsEmployeeHired();
            bool hasPr = divisions.PRManager.IsEmployeeHired();
            bool hasDe = divisions.DesignEngineer.IsEmployeeHired();
            if (hasBuilders && hasHr && hasPr && hasDe)
            {
                SetAchievement("ACH_E_MAX");
            }
        }
        private void OnConstructionTaskAccepted(ConstructionTaskData task)
        {
            if (task.Id == 131)
            {
                SetAchievement("ACH_DARK_B");
            }
        }
        private void OnConstructionBuilded(ConstructionData construction)
        {
            switch (construction.BuildingData.BuildingType)
            {
                case BuildingType.House: SetAchievement("ACH_B_HOUSE"); break;
                case BuildingType.HighRise: SetAchievement("ACH_B_HIGHRISE"); break;
            }
        }
        private void OnCollectibleFound(int _)
        {
            int totalFoundCollectibles = GameData.Data.EnvironmentData.Collectibles.Items.Count;
            if (totalFoundCollectibles >= EnvironmentData.TOTAL_COLLECTIBLES)
            {
                SetAchievement("ACH_C_MAX");
            }
        }
        private void OnMoneyChanged(int value, int changedAmount)
        {
            if (value >= 1000000)
            {
                SetAchievement("ACH_M_1000000");
            }
        }

        private void OnRatingChanged(int value, int changedAmount)
        {
            if (value >= 50)
            {
                SetAchievement("ACH_R_50");
            }
            if (value >= 100)
            {
                SetAchievement("ACH_R_100");
            }
        }

        public static void SetAchievement(string name)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement(name);
            SteamUserStats.StoreStats();
        }
        #endregion methods
    }
}