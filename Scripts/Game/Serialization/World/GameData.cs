using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class GameData
    {
        #region fields & properties
        public static readonly string SaveName = "save";
        public static readonly string SaveExtension = ".data";

        public static GameData Data => data;
        private static GameData data;

        public PlayerData PlayerData => playerData;
        [SerializeField] private PlayerData playerData = new();
        public CompanyData CompanyData => companyData;
        [SerializeField] private CompanyData companyData = new();

        public BrowserData BrowserData => browserData;
        [SerializeField] private BrowserData browserData = new();
        public LocationsData LocationsData => locationsData;
        [SerializeField] private LocationsData locationsData = new();
        public EnvironmentData EnvironmentData => environmentData;
        [SerializeField] private EnvironmentData environmentData = new();

        public BlueprintsData BlueprintsData => blueprintsData;
        [SerializeField] private BlueprintsData blueprintsData = new();
        public ConstructionsData ConstructionsData => constructionsData;
        [SerializeField] private ConstructionsData constructionsData = new();

        #region optimization
        [System.NonSerialized] private List<IBillPayable> _billPayables = null;
        [System.NonSerialized] private List<IMonthUpdatable> _monthUpdatables = null;
        [System.NonSerialized] private List<IMoodScaleHandler> _moodScalers = null;
        #endregion optimization
        #endregion fields & properties

        #region methods
        [Todo("Add IMoodScaleHandlers")]
        internal IReadOnlyList<IMoodScaleHandler> GetMoodScalers()
        {
            _moodScalers ??= new()
            {
                playerData.HouseData,
                playerData.VehicleData,
            };
            return _moodScalers;
        }

        [Todo("Add IBillPayables")]
        internal IReadOnlyList<IBillPayable> GetBillPayables()
        {
            _billPayables ??= new()
            {
                companyData.WarehouseData,
                companyData.OfficeData,
                companyData.OfficeData.Divisions,
                companyData.LandPlotsData,
                playerData.HouseData,
            };
            return _billPayables;
        }
        [Todo("Add IMonthUpdatables")]
        internal IReadOnlyList<IMonthUpdatable> GetMonthUpdatables()
        {
            _monthUpdatables ??= new()
            {
                browserData.ConstructionResourceShop,
                browserData.OfficeShop,
                browserData.WarehouseShop,
                browserData.LandPlotShop,
                browserData.BuildersRecruit,
                browserData.HRRecruit,
                browserData.PRRecruit,
                browserData.DesignEngineerRecruit,
                browserData.HouseShop,
                browserData.FoodShop,
                browserData.VehicleShop,
                environmentData,
                playerData.Mood,
                playerData.Tasks,
                playerData.BillsData,
                playerData.Food,
                companyData.ConstructionTasks,
                companyData.LandPlotsData,
            };
            return _monthUpdatables;
        }
        public static void SetData(GameData value) => data = value;
        #endregion methods
    }
}