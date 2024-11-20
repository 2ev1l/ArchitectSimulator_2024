using DebugStuff;
using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Environment
{
    public class VehicleLoader : MonoBehaviour
    {
        #region fields & properties
        private VehicleData Context => GameData.Data.PlayerData.VehicleData;
        public Transform ParentForSpawn => parentForSpawn;
        [SerializeField] private Transform parentForSpawn;
        [SerializeField] private bool loadHomePrefab = true;
        [System.NonSerialized] private GameObject spawnedVehicle = null;
        [System.NonSerialized] private int lastVehicleLoaded = -1;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            LoadVehicle();
            Context.OnInfoChanged += LoadVehicle;
        }
        private void OnDisable()
        {
            Context.OnInfoChanged -= LoadVehicle;
        }
        private void LoadVehicle()
        {
            int contextId = Context.Id;
            if (lastVehicleLoaded == contextId) return;
            SpawnVehicle();
            lastVehicleLoaded = contextId;
        }
        private void SpawnVehicle()
        {
            if (spawnedVehicle != null) Destroy(spawnedVehicle);
            if (Context.Id < 0) return;
            GameObject prefab = loadHomePrefab ? Context.Info.HousePrefab : Context.Info.CityPrefab;
            spawnedVehicle = Instantiate(prefab, parentForSpawn);
            spawnedVehicle.transform.localPosition = Vector3.zero;
        }
        #endregion methods
    }
}