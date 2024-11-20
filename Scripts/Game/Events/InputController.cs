using Game.Serialization.Settings;
using Game.Serialization.Settings.Input;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Universal.Core;
using Universal.Events;
using Zenject;

namespace Game.Events
{
    [System.Serializable]
    public class InputController : ITickable
    {
        #region fields & properties
        private static KeyCodeSettings Context => SettingsData.Data.KeyCodeSettings;
        private static InputController Instance;
        public static UnityAction<KeyCodeInfo> OnKeyDown;
        public static UnityAction<KeyCodeInfo> OnKeyUp;
        public static UnityAction<KeyCodeInfo> OnKeyHold;
        public static HashSet<KeyCodeInfo> AllKeys
        {
            get
            {
                allKeys ??= GetKeys(Context);
                return allKeys;
            }
        }
        private static HashSet<KeyCodeInfo> allKeys = null;
        private static HashSet<KeyCodeInfo> UIKeys
        {
            get
            {
                uiKeys ??= GetKeys(Context.UIKeys);
                return uiKeys;
            }
        }
        private static HashSet<KeyCodeInfo> uiKeys = null;
        private static HashSet<KeyCodeInfo> PlayerKeys
        {
            get
            {
                playerKeys ??= GetKeys(Context.PlayerKeys);
                return playerKeys;
            }
        }
        private static HashSet<KeyCodeInfo> playerKeys = null;
        private static HashSet<KeyCodeInfo> OverlayKeys
        {
            get
            {
                overlayKeys ??= GetKeys(Context.OverlayKeys);
                return overlayKeys;
            }
        }
        private static HashSet<KeyCodeInfo> overlayKeys = null;
        private static KeyCodeInfo SettingsStaticKey
        {
            get
            {
                if (settingsStaticKey == null)
                    UIKeys.Exists(x => x.Description == KeyCodeDescription.OpenSettings, out settingsStaticKey);
                return settingsStaticKey;
            }
        }
        private static KeyCodeInfo settingsStaticKey = null;
        private static GameObject EventSystem
        {
            get
            {
                if (eventSystem == null)
                {
                    eventSystem = GameObject.FindAnyObjectByType<EventSystem>().gameObject;
                }
                return eventSystem;
            }
        }
        private static GameObject eventSystem;

        //non static fields for debugging
        private static ActionRequest InputPlayer => Instance.inputPlayer;
        [SerializeField] private ActionRequest inputPlayer = new();
        private static ActionRequest InputUI => Instance.inputUI;
        [SerializeField] private ActionRequest inputUI = new();
        private static ActionRequest InputOverlay => Instance.inputOverlay;
        [SerializeField] private ActionRequest inputOverlay = new();
        private static ActionRequest InputEventSystem => Instance.inputEventSystem;
        [SerializeField] private ActionRequest inputEventSystem = new();
        #endregion fields & properties

        #region methods
        public void ForceInitialize()
        {
            Instance = this;
        }
        private static HashSet<KeyCodeInfo> GetKeys(KeysCollection referenceCollection)
        {
            return referenceCollection.GetKeys().ToHashSet();
        }
        public static void LockInputSystem(int blockLevel)
        {
            InputEventSystem.AddBlockLevel(blockLevel);
            if (!EventSystem.activeSelf) return;
            EventSystem.SetActive(false);
        }
        public static void UnlockInputSystem(int blockLevel)
        {
            InputEventSystem.RemoveBlockLevel(blockLevel);
            bool canExecute = InputEventSystem.CanExecute(int.MinValue);
            if (EventSystem.activeSelf == canExecute) return;
            EventSystem.SetActive(canExecute);
        }
        public static void LockFullInput(int blockLevel)
        {
            InputUI.AddBlockLevel(blockLevel);
            InputPlayer.AddBlockLevel(blockLevel);
            InputOverlay.AddBlockLevel(blockLevel);
        }
        public static void UnlockFullInput(int blockLevel)
        {
            InputUI.RemoveBlockLevel(blockLevel);
            InputPlayer.RemoveBlockLevel(blockLevel);
            InputOverlay.RemoveBlockLevel(blockLevel);
        }

        public static void LockPlayerInput(int blockLevel)
        {
            InputPlayer.AddBlockLevel(blockLevel);
        }
        public static void UnlockPlayerInput(int blockLevel)
        {
            InputPlayer.RemoveBlockLevel(blockLevel);
        }

        public static void LockUIInput(int blockLevel)
        {
            InputUI.AddBlockLevel(blockLevel);
        }
        public static void UnlockUIInput(int blockLevel)
        {
            InputUI.RemoveBlockLevel(blockLevel);
        }

        public static void LockOverlayInput(int blockLevel)
        {
            InputOverlay.AddBlockLevel(blockLevel);
        }
        public static void UnlockOverlayInput(int blockLevel)
        {
            InputOverlay.RemoveBlockLevel(blockLevel);
        }
        private static void Update()
        {
            CheckInputUI();
            CheckInputPlayer();
            CheckOverlayInput();
        }
        private static void CheckInputUI()
        {
            if (!InputUI.CanExecute(0))
            {
                if (!InputUI.CanExecute(int.MaxValue)) return;
                CheckKeyActions(SettingsStaticKey);
                return;
            }
            CheckKeysActions(UIKeys);
        }
        private static void CheckOverlayInput()
        {
            if (!InputOverlay.CanExecute(0)) return;
            CheckKeysActions(OverlayKeys);
        }
        private static void CheckInputPlayer()
        {
            if (!InputPlayer.CanExecute(0)) return;
            CheckKeysActions(PlayerKeys);
        }
        private static void CheckKeysActions(HashSet<KeyCodeInfo> keys)
        {
            foreach (KeyCodeInfo key in keys)
                CheckKeyActions(key);
        }
        private static void CheckKeyActions(KeyCodeInfo key)
        {
            if (Input.GetKeyDown(key.Key))
                OnKeyDown?.Invoke(key);

            if (Input.GetKeyUp(key.Key))
                OnKeyUp?.Invoke(key);

            if (Input.GetKey(key.Key))
                OnKeyHold?.Invoke(key);
        }
        public static bool IsKeyHold(KeyCode key)
        {
            return Input.GetKey(key);
        }
        public void Tick() => Update();
        #endregion methods
    }
}