using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class MonthRandomActions
    {
        #region fields & properties
        internal static readonly LanguageInfo vehicleWheelRepairInfo = new(435, TextType.Game);
        internal static readonly LanguageInfo vehicleGlassRepairInfo = new(436, TextType.Game);
        internal static readonly LanguageInfo vehicleTaxReturnInfo = new(437, TextType.Game);

        internal static readonly LanguageInfo houseRobberyInfo = new(438, TextType.Game);
        internal static readonly LanguageInfo houseTaxReturnInfo = new(439, TextType.Game);

        internal static readonly LanguageInfo employeeFireInfo = new(440, TextType.Game);

        internal static readonly LanguageInfo companyCompetitorsNegativeInfo = new(441, TextType.Game);
        internal static readonly LanguageInfo companyCompetitorsPositiveInfo = new(442, TextType.Game);

        internal static readonly LanguageInfo darkBusinessPoliceNegativeCheckInfo = new(443, TextType.Game);
        internal static readonly LanguageInfo darkBusinessPoliceNeutralCheckInfo = new(444, TextType.Game);
        internal static readonly LanguageInfo darkBusinessInterestIncomeInfo = new(445, TextType.Game);

        internal static readonly IReadOnlyList<int> darkBusinessTasks = new List<int>()
        {
            31, 44, 62, 68, 97, 108, 110, 131, 141, 146, 147, 158, 159, 160, 163, 173, 177, 187
        };
        #endregion fields & properties

        #region methods
        public bool TryGetAction(out MonthAction action)
        {
            action = null;
            if (!CanGetAction()) return false;
            action = GetRandomAction();
            return action != null;
        }
        private bool CanGetAction()
        {
            int currentMonth = GameData.Data.PlayerData.MonthData.CurrentMonth;
            if (currentMonth < 15 || GameData.Data.CompanyData.Rating.Value < 15) return false;
            float chance = Mathf.Clamp(Mathf.Pow(currentMonth, 1.1f), 0, 35);
            return CustomMath.GetRandomChance(chance);
        }
        private MonthAction GetRandomAction()
        {
            if (TryGetEmployeesRandomAction(out MonthAction employeesAction)) return employeesAction;
            if (TryGetCompanyRandomAction(out MonthAction companyAction)) return companyAction;
            if (TryGetHouseRandomAction(out MonthAction houseAction)) return houseAction;
            if (TryTasksDataRandomAction(out MonthAction tasksDataAction)) return tasksDataAction;
            if (TryGetVehicleRandomAction(out MonthAction vehicleAction)) return vehicleAction;

            return null;
        }
        public bool TryGetVehicleRandomAction(out MonthAction action)
        {
            action = null;
            BuyableVehicle vehicle = (BuyableVehicle)GameData.Data.PlayerData.VehicleData.BuyableInfo;
            if (vehicle != null)
            {
                int vehiclePrice = vehicle.Price;
                switch (CustomMath.GetRandomChance())
                {
                    case float i when i <= 10: action = GetVehicleWheelRepairAction(vehiclePrice); break;
                    case float i when i <= 20: action = GetVehicleGlassRepairAction(vehiclePrice); break;
                    case float i when i <= 30: action = GetVehicleTaxReturnAction(vehiclePrice); break;
                }
            }
            return action != null;
        }
        public MonthAction GetVehicleWheelRepairAction(int vehiclePrice) => new(-vehiclePrice / 30, 0, -3, vehicleWheelRepairInfo);
        public MonthAction GetVehicleGlassRepairAction(int vehiclePrice) => new(-vehiclePrice / 20, 0, -4, vehicleGlassRepairInfo);
        public MonthAction GetVehicleTaxReturnAction(int vehiclePrice) => new(vehiclePrice / 15, 0, 4, vehicleTaxReturnInfo);
        public bool TryGetHouseRandomAction(out MonthAction action)
        {
            action = null;
            RentableHouse house = (RentableHouse)GameData.Data.PlayerData.HouseData.RentableInfo;
            if (house != null)
            {
                int housePrice = house.Price;
                switch (CustomMath.GetRandomChance())
                {
                    case float i when i <= 9: action = GetHouseRobberyAction(housePrice); break;
                    case float i when i <= 20: action = GetHouseTaxReturnAction(housePrice); break;
                }
            }
            return action != null;
        }
        public MonthAction GetHouseRobberyAction(int housePrice) => new(-housePrice / 50, 0, -3, houseRobberyInfo);
        public MonthAction GetHouseTaxReturnAction(int housePrice) => new(housePrice / 30, 0, 4, houseTaxReturnInfo);

        public bool TryGetEmployeesRandomAction(out MonthAction action)
        {
            action = null;
            OfficeData office = GameData.Data.CompanyData.OfficeData;
            List<BuilderData> builders = office.Divisions.Builders.Employees.Select(x => (BuilderData)x).Where(x => !x.IsBusy).ToList();
            if (builders.Count > 0)
            {
                switch (CustomMath.GetRandomChance())
                {
                    case float i when i <= 8: GetFireBuilderAction(builders, true); break;
                }
            }
            return action != null;
        }
        public MonthAction GetFireBuilderAction(List<BuilderData> builders, bool fire = true)
        {
            OfficeData office = GameData.Data.CompanyData.OfficeData;
            int rndBuilder = Random.Range(0, builders.Count);
            if (fire)
                office.FireBuilder(builders[rndBuilder]);
            return new MonthAction(0, -1, -3, employeeFireInfo);
        }

        public bool TryGetCompanyRandomAction(out MonthAction action)
        {
            action = null;
            switch (CustomMath.GetRandomChance())
            {
                case float i when i <= 7: action = GetCompanyComeptitorsNegativeAction(); break;
                case float i when i <= 15: action = GetCompanyComeptitorsPositiveAction(); break;
            }
            return action != null;
        }
        public MonthAction GetCompanyComeptitorsNegativeAction() => new(0, -2, -3, companyCompetitorsNegativeInfo);
        public MonthAction GetCompanyComeptitorsPositiveAction() => new(0, 2, 5, companyCompetitorsPositiveInfo);

        public bool TryTasksDataRandomAction(out MonthAction action)
        {
            action = null;
            PlayerTasksData playerTasks = GameData.Data.PlayerData.Tasks;
            ConstructionTasksData constructionTasks = GameData.Data.CompanyData.ConstructionTasks;
            bool darkBusinessCompleted = false;
            int darkBusinessRequired = 4;
            int darkBusinessCompletedCount = 0;
            foreach (int id in darkBusinessTasks)
            {
                if (!constructionTasks.IsTaskCompleted(id, out _)) continue;
                darkBusinessCompletedCount++;
                if (darkBusinessCompletedCount < darkBusinessRequired) continue;
                darkBusinessCompleted = true;
                break;
            }
            int playerMoney = GameData.Data.PlayerData.Wallet.Value;

            if (darkBusinessCompleted)
            {
                switch (CustomMath.GetRandomChance())
                {
                    case float i when i <= 9: action = GetDarkBusinessPoliceNegativeCheckAction(playerMoney); break;
                }
            }

            switch (CustomMath.GetRandomChance())
            {
                case float i when i <= 10: action = GetDarkBusinessPoliceNeutralCheckAction(); break;
            }

            if (playerTasks.IsTaskCompleted(35, out _))
            {
                switch (CustomMath.GetRandomChance())
                {
                    case float i when i <= 10: action = GetDarkBusinessInterestIncomeAction(); break;
                }
            }

            return action != null;
        }
        public MonthAction GetDarkBusinessPoliceNegativeCheckAction(int playerMoney) => new(-playerMoney / 10, -1, -4, darkBusinessPoliceNegativeCheckInfo);
        public MonthAction GetDarkBusinessPoliceNeutralCheckAction() => new(0, 1, 5, darkBusinessPoliceNeutralCheckInfo);
        public MonthAction GetDarkBusinessInterestIncomeAction()
        {
            int rndInterest = Random.Range(10000, 50000);
            return new(rndInterest, 0, 0, darkBusinessInterestIncomeInfo);
        }
        #endregion methods

        public class MonthAction
        {
            public int MoneyChange { get; }
            public int RatingChange { get; }
            public int MoodChange { get; }
            public LanguageInfo Description { get; }

            public MonthAction(int moneyChange, int ratingChange, int moodChange, LanguageInfo description)
            {
                MoneyChange = moneyChange;
                RatingChange = ratingChange;
                MoodChange = moodChange;
                Description = description;
            }
        }
    }
}