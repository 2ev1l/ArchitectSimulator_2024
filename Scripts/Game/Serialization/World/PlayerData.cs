using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class PlayerData
    {
        #region fields & properties
        public MonthData MonthData => monthData;
        [SerializeField] private MonthData monthData = new();
        public Wallet Wallet => wallet;
        [SerializeField] private Wallet wallet = new(4500);
        public MoodData Mood => mood;
        [SerializeField] private MoodData mood = new();
        public FoodData Food => food;
        [SerializeField] private FoodData food = new();

        public PlayerTasksData Tasks => tasks;
        [SerializeField] private PlayerTasksData tasks = new();
        public SubtitlesData SubtitlesData => subtitlesData;
        [SerializeField] private SubtitlesData subtitlesData = new();
        public BillsData BillsData => billsData;
        [SerializeField] private BillsData billsData = new();

        public HouseData HouseData => houseData;
        [SerializeField] private HouseData houseData = new(0);
        public VehicleData VehicleData => vehicleData;
        [SerializeField] private VehicleData vehicleData = new(-1);
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}