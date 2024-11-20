using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;
using Universal.Serialization;

namespace Game.Serialization.World
{
    internal sealed class GameDataDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        #region fields & properties
        [SerializeField] private GameData Context = null;
        [Title("Debug")]
        [SerializeField] private bool doAutoSave = false;
        [SerializeField][DrawIf(nameof(doAutoSave), true)][Min(1)] private int autoSaveSeconds = 60;
        [SerializeField][ReadOnly] private float storedTime = 0;
        [SerializeField][Min(0)] private int taskToDebug = 0;
        [SerializeField][Min(0)] private int consturctionToDebug = 0;
        [SerializeField][Min(0)] private int houseToDebug = 1;
        [SerializeField][Min(0)] private int vehicleToDebug = 1;
        [SerializeField][Min(0)] private int collectibleToDebug = 1;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            UpdateData();
            SavingUtils.OnDataReset += UpdateData;
        }
        private void OnDisable()
        {
            SavingUtils.OnDataReset -= UpdateData;
        }
        private void Update()
        {
            if (doAutoSave)
            {
                storedTime += Time.deltaTime;
                if (storedTime > autoSaveSeconds)
                {
                    storedTime = 0;
                    SavingUtils.Instance.SaveGameData();
                }
            }
        }
        [Button(nameof(UpdateData))]
        private void UpdateData()
        {
            Context = GameData.Data;
        }
        [Button(nameof(AddAllResourcesToWarehouse))]
        private void AddAllResourcesToWarehouse()
        {
            foreach (var el in DB.Instance.ConstructionResourceInfo.Data)
            {
                if (!Context.CompanyData.WarehouseData.TryAddConstructionResource(el.Id, 900))
                    Debug.Log($"Resource #{el.Id} wasn't added");
            }
        }
        [Button(nameof(GenerateLandPlotOffers))]
        private void GenerateLandPlotOffers()
        {
            Context.CompanyData.LandPlotsData.GenerateOffers();
        }
        [Button(nameof(BuildConstruction))]
        private void BuildConstruction()
        {
            Context.ConstructionsData.TryBuildByBaseId(consturctionToDebug);
        }
        [Button(nameof(StartNextMonth))]
        private void StartNextMonth()
        {
            Context.PlayerData.MonthData.StartNextMonth();
        }
        [Button(nameof(CollectBills))]
        private void CollectBills()
        {
            Context.PlayerData.BillsData.AddBills();
        }
        [Button(nameof(PayBills))]
        private void PayBills()
        {
            Context.PlayerData.BillsData.TryPayBills();
        }
        [Button(nameof(CompleteTask))]
        private void CompleteTask()
        {
            Context.PlayerData.Tasks.TryCompleteTask(taskToDebug);
        }
        [Button(nameof(StartTask))]
        private void StartTask()
        {
            Context.PlayerData.Tasks.TryStartTask(taskToDebug);
        }
        [Button(nameof(StartConstructionTask))]
        private void StartConstructionTask()
        {
            Context.CompanyData.ConstructionTasks.TryStartTask(taskToDebug);
        }
        [Button(nameof(ClearSubtitles))]
        private void ClearSubtitles()
        {
            Context.PlayerData.SubtitlesData.ClearAllPlayedSubtitles();
        }
        [Button(nameof(SetHouse))]
        private void SetHouse()
        {
            Context.PlayerData.HouseData.TryReplaceInfo(houseToDebug);
        }
        [Button(nameof(IncreaseRating))]
        private void IncreaseRating()
        {
            Context.CompanyData.Rating.TryIncreaseValue(1);
        }
        [Button(nameof(SetVehicle))]
        private void SetVehicle()
        {
            Context.PlayerData.VehicleData.TryReplaceInfo(vehicleToDebug);
        }
        [Button(nameof(AddCollectible))]
        private void AddCollectible()
        {
            Context.EnvironmentData.Collectibles.TryAddItem(collectibleToDebug, x => x == collectibleToDebug);
        }
        [Button(nameof(SetCurrentDataAsReal))]
        private void SetCurrentDataAsReal()
        {
            GameData.SetData(Context);
        }
        [Button(nameof(SAVEDATA))]
        private void SAVEDATA()
        {
            SavingUtils.Instance.SaveGameData();
        }
        #endregion methods
#endif //UNITY_EDITOR
    }
}