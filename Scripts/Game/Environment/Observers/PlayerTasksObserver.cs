using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public class PlayerTasksObserver : TasksObserver<PlayerTaskData, PlayerTaskInfo>
    {
        #region fields & properties
        protected override TasksData<PlayerTaskData, PlayerTaskInfo> Context => GameData.Data.PlayerData.Tasks;
        private ConstructionTasksData ConstructionTasks => GameData.Data.CompanyData.ConstructionTasks;
        #endregion fields & properties

        #region methods
        public override void Dispose()
        {
            base.Dispose();
            GameData.Data.CompanyData.OnCreated -= OnCompanyCreated;
            GameData.Data.LocationsData.OnLocationChanged -= OnLocationChanged;
            LocationObserver.OnLocationLoaded -= OnLocationChanged;
            GameData.Data.CompanyData.WarehouseData.OnInfoChanged -= OnWarehouseChanged;
            GameData.Data.CompanyData.OfficeData.OnInfoChanged -= OnOfficeChanged;
            GameData.Data.CompanyData.OfficeData.OnEmployeeHired -= OnEmployeeHired;
            GameData.Data.CompanyData.ConstructionTasks.OnTaskAccepted -= OnCosnstructionTaskAccepted;
            ConstructionTasks.OnTaskRejected -= OnConstructionTaskRejected;
            GameData.Data.ConstructionsData.OnConstructionAdded -= OnConstructionAdded;
            GameData.Data.ConstructionsData.OnConstructionBuilded -= OnConstructionBuilded;
            GameData.Data.ConstructionsData.OnConstructionBuildStarted -= OnConstructionBuildStarted;
            GameData.Data.PlayerData.MonthData.OnMonthChanged -= OnMonthChanged;
            GameData.Data.PlayerData.BillsData.OnBillPayed -= OnBillPayed;
            GameData.Data.PlayerData.Food.OnSaturationIncreased -= OnSaturationIncreased;
            GameData.Data.PlayerData.VehicleData.OnInfoChanged -= OnVehicleChanged;
            GameData.Data.PlayerData.HouseData.OnInfoChanged -= OnHouseChanged;
            GameData.Data.PlayerData.Wallet.OnValueChanged -= OnMoneyChanged;
            GameData.Data.CompanyData.Rating.OnValueChanged -= OnRatingChanged;
            GameData.Data.EnvironmentData.Collectibles.OnItemAdded -= OnCollectibleFound;
            GameData.Data.CompanyData.LandPlotsData.OnPlotSold -= OnLandPlotSold;
        }

        public override void Initialize()
        {
            base.Initialize();
            GameData.Data.CompanyData.OnCreated += OnCompanyCreated;
            GameData.Data.LocationsData.OnLocationChanged += OnLocationChanged;
            LocationObserver.OnLocationLoaded += OnLocationChanged;
            GameData.Data.CompanyData.WarehouseData.OnInfoChanged += OnWarehouseChanged;
            GameData.Data.CompanyData.OfficeData.OnInfoChanged += OnOfficeChanged;
            GameData.Data.CompanyData.OfficeData.OnEmployeeHired += OnEmployeeHired;
            GameData.Data.CompanyData.ConstructionTasks.OnTaskAccepted += OnCosnstructionTaskAccepted;
            ConstructionTasks.OnTaskRejected += OnConstructionTaskRejected;
            GameData.Data.ConstructionsData.OnConstructionAdded += OnConstructionAdded;
            GameData.Data.ConstructionsData.OnConstructionBuilded += OnConstructionBuilded;
            GameData.Data.ConstructionsData.OnConstructionBuildStarted += OnConstructionBuildStarted;
            GameData.Data.PlayerData.MonthData.OnMonthChanged += OnMonthChanged;
            GameData.Data.PlayerData.BillsData.OnBillPayed += OnBillPayed;
            GameData.Data.PlayerData.Food.OnSaturationIncreased += OnSaturationIncreased;
            GameData.Data.PlayerData.VehicleData.OnInfoChanged += OnVehicleChanged;
            GameData.Data.PlayerData.HouseData.OnInfoChanged += OnHouseChanged;
            GameData.Data.PlayerData.Wallet.OnValueChanged += OnMoneyChanged;
            GameData.Data.CompanyData.Rating.OnValueChanged += OnRatingChanged;
            GameData.Data.EnvironmentData.Collectibles.OnItemAdded += OnCollectibleFound;
            GameData.Data.CompanyData.LandPlotsData.OnPlotSold += OnLandPlotSold;

            VerifyDelayedTasks();
        }
        private void VerifyDelayedTasks()
        {
            TryStartTaskDelayed(0);
            if (Context.IsTaskCompleted(9, out _)) TryStartTaskDelayed(10);

        }

        protected override void OnTaskStarted(PlayerTaskData taskData)
        {
            ShowTaskStartedMessage(taskData);
            int taskId = taskData.Id;
            switch (taskId)
            {
                case 4:
                    ConstructionTasks.TryStartTask(0);
                    break;
                case 9:
                    ConstructionTasks.TryStartTask(2);
                    break;
                case 10:
                    ConstructionTasks.TryStartTask(1);
                    break;
                case 14:
                    if (GameData.Data.CompanyData.Rating.Value >= 2)
                        TryCompleteTask(14);
                    break;
                case 15:
                    ConstructionTasks.TryStartTask(3);
                    if (GameData.Data.CompanyData.WarehouseData.Id == 0)
                        TryStartTask(16);
                    break;
                case 19:
                    ConstructionTasks.TryStartTask(15);
                    break;
                case 21:
                    ConstructionTasks.TryStartTask(32);
                    break;
                case 23:
                    if (GameData.Data.CompanyData.OfficeData.Divisions.PRManager.IsEmployeeHired())
                        TryCompleteTask(23);
                    break;
                case 24:
                    ConstructionTasks.TryStartTask(52);
                    break;
                case 26:
                    if (GameData.Data.EnvironmentData.Collectibles.Items.Count >= 5)
                        TryCompleteTask(26);
                    break;
                case 28:
                    if (GameData.Data.CompanyData.Rating.Value >= 60)
                        TryCompleteTask(28);
                    break;
                case 29:
                    if (GameData.Data.PlayerData.Wallet.Value >= 100000)
                        TryCompleteTask(29);
                    break;
                case 30:
                    if (GameData.Data.EnvironmentData.Collectibles.Items.Count >= EnvironmentData.TOTAL_COLLECTIBLES)
                        TryCompleteTask(30);
                    break;
                case 31:
                    if (GameData.Data.PlayerData.Wallet.Value >= 1000000)
                        TryCompleteTask(31);
                    break;
                case 33:
                    if (GameData.Data.CompanyData.LandPlotsData.SoldPlots.Count >= 5)
                        TryCompleteTask(33);
                    break;
                case 35:
                    if (GameData.Data.CompanyData.ConstructionTasks.IsTaskCompleted(131, out _))
                        TryCompleteTask(35);
                    break;
                case 36:
                    ConstructionTasks.TryStartTask(150);
                    break;
                case 39:
                    if (GameData.Data.CompanyData.Rating.Value >= 100)
                        TryCompleteTask(39);
                    break;
            }
        }
        /// <summary>
        /// More accurate for debug than writing right after complete condition
        /// </summary>
        /// <param name="taskData"></param>
        protected override void OnTaskCompleted(PlayerTaskData taskData)
        {
            int taskId = taskData.Id;
            switch (taskId)
            {
                case 9: TryStartTaskDelayed(10); break;
                case 27:
                    int houseId = GameData.Data.PlayerData.HouseData.Id;
                    if (houseId <= 0 || houseId == 5)
                        TryStartTask(32);
                    break;
            }
        }
        protected override void OnTaskExpired(PlayerTaskData taskData)
        {
            int taskId = taskData.Id;
            switch (taskId)
            {
                case 20: TryStartTask(21); break;
            }
        }

        private void OnLandPlotSold(LandPlotData soldPlot)
        {
            TryCompleteTask(27);
            if (GameData.Data.CompanyData.LandPlotsData.SoldPlots.Count >= 5)
            {
                TryCompleteTask(33);
            }
        }
        private void OnCollectibleFound(int collectibleId)
        {
            int totalFoundCollectibles = GameData.Data.EnvironmentData.Collectibles.Items.Count;
            if (totalFoundCollectibles >= 5)
                TryCompleteTask(26);

            if (totalFoundCollectibles >= EnvironmentData.TOTAL_COLLECTIBLES)
                TryCompleteTask(30);
        }
        private void OnMoneyChanged(int currentValue, int changedAmount)
        {
            if (currentValue <= 400 && GameData.Data.CompanyData.Rating.Value < 15 && GameData.Data.PlayerData.HouseData.Id != 5)
            {
                TryStartTask(18);
            }
            if (currentValue >= 100000)
            {
                TryCompleteTask(29);
            }
            if (currentValue >= 1000000)
            {
                TryCompleteTask(31);
            }
        }
        private void OnRatingChanged(int totalAmount, int changedAmount)
        {
            if (totalAmount >= 2)
            {
                TryCompleteTask(14);
            }
            else
            {
                if (Context.IsTaskStarted(15, out _) && changedAmount < 0)
                {
                    int minReq = 2;
                    GameData.Data.CompanyData.Rating.TryIncreaseValue(minReq - totalAmount);
                }
            }
            if (totalAmount >= 60)
            {
                TryCompleteTask(28);
            }
            if (totalAmount >= 100)
            {
                TryCompleteTask(39);
            }
        }
        private void OnMonthChanged(int currentMonth)
        {
            TryCompleteTask(6);
            TryCompleteTask(12);

            void CompleteOnLastExpirationMonth(int taskId)
            {
                if (Context.IsTaskStarted(taskId, out PlayerTaskData task))
                {
                    if (task.IsLastMonthBeforeExpiration())
                    {
                        TryCompleteTask(taskId);
                    }
                }
            }
            CompleteOnLastExpirationMonth(17);
            CompleteOnLastExpirationMonth(22);
            CompleteOnLastExpirationMonth(25);
            CompleteOnLastExpirationMonth(37);
        }

        private void OnCosnstructionTaskAccepted(ConstructionTaskData task)
        {
            if (task.Id == 131)
            {
                TryCompleteTask(35);
            }
        }
        private void OnConstructionAdded(ConstructionData construction)
        {
            TryCompleteTask(4);
        }
        private void OnConstructionBuildStarted(ConstructionData construction)
        {
            TryCompleteTask(5);
        }
        private void OnConstructionBuilded(ConstructionData construction)
        {
            TryCompleteTask(8);
            int blueprintId = construction.BlueprintInfoId;
            switch (blueprintId)
            {
                case 1: TryCompleteTask(10); break;
                case 3: TryCompleteTask(15); break;
                case 15: TryCompleteTask(19); break;
                case 32: TryCompleteTask(21); break;
                case 52: TryCompleteTask(24); break;
                case 155: TryCompleteTask(36); break;
            }
            BuildingType buildingType = construction.BuildingData.BuildingType;
            if (buildingType == BuildingType.ApartmentBuilding || buildingType == BuildingType.HighRise)
            {
                TryCompleteTask(38);
            }
        }
        private void OnConstructionTaskRejected(ConstructionTaskData taskData)
        {
            TryCompleteTask(9);
        }
        private void OnEmployeeHired(EmployeeData employee)
        {
            if (employee is PRManagerData pr)
            {
                TryCompleteTask(23);
            }
        }
        private void OnSaturationIncreased(int totalSaturation, int increasedSaturation)
        {
            TryCompleteTask(13);
            if (increasedSaturation == 50)
            {
                TryCompleteTask(34);
            }
        }
        private void OnOfficeChanged()
        {
            TryCompleteTask(1);
        }
        private void OnWarehouseChanged()
        {
            TryCompleteTask(3);
        }
        private void OnHouseChanged()
        {
            int currentHouse = GameData.Data.PlayerData.HouseData.Id;
            if (currentHouse == 5) TryCompleteTask(18);
            TryCompleteTask(32);
        }
        private void OnVehicleChanged()
        {
            TryCompleteTask(20);
        }
        private void OnLocationChanged(int locationId)
        {
            if (locationId == 1)
            {
                TryCompleteTask(2);
            }
        }
        private void OnBillPayed()
        {
            TryCompleteTask(7);
        }
        private void OnCompanyCreated()
        {
            TryCompleteTask(0);
        }

        private void TryStartTaskDelayed(int taskId, float delay = 1f) => SingleGameInstance.Instance.StartCoroutine(TryStartTaskDelayedIEnumerator(taskId, delay));
        private IEnumerator TryStartTaskDelayedIEnumerator(int taskId, float delay = 1f)
        {
            if (!Context.CanStartTask(taskId))
                yield break;
            yield return new WaitForSeconds(delay);
            TryStartTask(taskId);
        }

        private void ShowTaskStartedMessage(PlayerTaskData taskData)
        {
            SubtitleRequest subtitleRequest = new(taskData.Info.StartSubtitlesTrigger.ToArray());
            subtitleRequest.Send();
        }

        #endregion methods
    }
}