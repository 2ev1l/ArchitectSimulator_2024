using DebugStuff;
using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Overlay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Game.Environment
{
    public class HouseLoader : MonoBehaviour
    {
        #region fields & properties
        private HouseData HouseData => GameData.Data.PlayerData.HouseData;
        private bool MustBeDynamic => InfoReference.Id == HouseData.Id;
        public HouseInfo InfoReference => infoReference.Data;
        [SerializeField] private HouseInfoSO infoReference;
        public StaticHousePrefab StaticPrefab => staticPrefab;
        [SerializeField] private StaticHousePrefab staticPrefab;
        public DynamicHousePrefab DynamicPrefab => dynamicPrefab;
        [SerializeField] private DynamicHousePrefab dynamicPrefab;
        [SerializeField] private StateMachine houseStateMachine;

        [System.NonSerialized] private bool loadedStaticPrefab = false;
        [System.NonSerialized] private bool loadedDynamicPrefab = false;
        [Inject] private Player.Input playerInput;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            HouseData.OnInfoChanged += LoadHouseOnAction;
            LoadHouseOnAction();
        }
        private void OnDisable()
        {
            HouseData.OnInfoChanged -= LoadHouseOnAction;
        }
        private void LoadHouseOnAction()
        {
            bool wasLoaded = loadedDynamicPrefab;
            LoadHouse();
            if (loadedDynamicPrefab && !wasLoaded)
            {
                playerInput.Moving.TeleportToIgnoreLayer(dynamicPrefab.SafePlayerPosition.position, Physics.AllLayers);
            }
        }
        private void LoadHouse()
        {
            if (MustBeDynamic)
            {
                if (!loadedDynamicPrefab)
                    LoadDynamicPrefab();
            }
            else
            {
                if (!loadedStaticPrefab)
                    LoadStaticPrefab();
            }
        }
        private void LoadStaticPrefab()
        {
            houseStateMachine.TryApplyState(staticPrefab);
            loadedStaticPrefab = true;
            loadedDynamicPrefab = false;
        }
        private void LoadDynamicPrefab()
        {
            houseStateMachine.TryApplyState(dynamicPrefab);
            loadedStaticPrefab = false;
            loadedDynamicPrefab = true;
            UIStateMachine.Instance.ApplyDefaultState();
        }
        #endregion methods

#if UNITY_EDITOR
        [Button(nameof(GetAllStates))]
        private void GetAllStates()
        {
            UnityEditor.Undo.RecordObject(this, "Set states");
            this.houseStateMachine.ReplaceStates(GetComponentsInChildren<StateChange>(true).ToList());
        }
        [Button(nameof(DebugHouseLoaders))]
        private void DebugHouseLoaders()
        {
            var loaders = GameObject.FindObjectsByType<HouseLoader>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<int> infos = DB.Instance.HouseInfo.Data.Select(x => x.Data.Id).ToList();
            foreach (var el in loaders)
            {
                if (el.DynamicPrefab == null)
                    LogLoader($"Dynamic prefab is null", el);
                if (el.StaticPrefab == null)
                    LogLoader($"Static prefab is null", el);

                CheckStates(el);
                if (el.StaticPrefab.Root == null)
                    LogLoader($"Static prefab : Root is null", el);
                if (el.DynamicPrefab.Root == null)
                    LogLoader($"Dynamic prefab : Root is null", el);

                if (el.DynamicPrefab.SafePlayerPosition == null)
                    LogLoader($"Dynamic prefab : SafePlayer position is null", el);
                if (el.DynamicPrefab.Bed == null)
                    LogLoader($"Dynamic prefab : Bed is null", el);
                if (el.DynamicPrefab.Laptop == null)
                    LogLoader($"Dynamic prefab : Laptop is null", el);
                if (el.DynamicPrefab.Shoes == null)
                    LogLoader($"Dynamic prefab : Shoues is null", el);
                if (el.DynamicPrefab.VehicleLoader == null)
                    LogLoader($"Dynamic prefab : Vehicle loader is null", el);
                if (el.DynamicPrefab.VehicleLoader.ParentForSpawn == null)
                    LogLoader($"Dynamic prefab : Vehicle loader : Parent for spawn is null", el);

                if (infos.Contains(el.InfoReference.Id))
                    infos.Remove(el.InfoReference.Id);
                else
                    LogLoader("Duplicate or non-exist info reference", el);
            }

            if (infos.Count == 0) return;
            StringBuilder sb = new();
            sb.Append($"Missing infos: ");
            foreach (int el in infos)
            {
                sb.AppendLine($"#{el}");
            }
        }
        private void CheckStates(HouseLoader el)
        {
            if (el.houseStateMachine.States.Count != 2)
            {
                LogLoader($"States count is wrong", el);
                return;
            }
            StateChange state0 = el.houseStateMachine.States[0];
            StateChange state1 = el.houseStateMachine.States[1];
            if (state0 == el.dynamicPrefab && state1 == el.staticPrefab)
            {
                return;
            }
            if (state0 == el.staticPrefab && state1 == el.dynamicPrefab)
            {
                return;
            }
            LogLoader($"States is wrong", el);
        }
        private static void LogLoader(string log, HouseLoader loader) => Debug.LogError($"{loader.name} : <color=#FFCCCC>{log}</color>", loader);
#endif //UNITY_EDITOR

    }
}