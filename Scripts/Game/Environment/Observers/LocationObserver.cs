using Game.Events;
using Game.Serialization.World;
using Game.UI.Overlay;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Universal.Behaviour;
using Universal.Serialization;
using Zenject;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public class LocationObserver : Observer
    {
        #region fields & properties
        private static LocationObserver Instance;
        public static UnityAction<int> OnLocationLoaded;
        public LocationStateChange CurrentLocation => (LocationStateChange)locationStates.CurrentState;
        [SerializeField] private StateMachine locationStates = new();
        private readonly Universal.Time.Timer nextLocationTimer = new();
        private bool isInputLocked = false;
        [Inject] private ScreenFade screenFade;
        [Inject] private Player.Input playerInput;
        #endregion fields & properties

        #region methods
        public override void Initialize()
        {
            Instance = this;
            int currentLocation = GameData.Data.LocationsData.CurrentLocationId;
            locationStates.TryApplyState(currentLocation);
            SetLocation(locationStates.CurrentState);
            locationStates.OnStateChanged += SetLocation;
        }
        public override void Dispose()
        {
            locationStates.OnStateChanged -= SetLocation;
        }
        public static void MoveToLocation(int locationId)
        {
            Instance.screenFade.DoCycle();
            SingleGameInstance.Instance.StartCoroutine(Instance.WaitForSmoothLocationChange());
            Instance.LockInput();

            Instance.nextLocationTimer.OnChangeEnd = delegate
            {
                try
                {
                    Instance.locationStates.TryApplyState(locationId);
                    Instance.UnlockInput();
                }
                catch
                {
                    Debug.LogError($"Can't change location to {locationId}. Set to default");
                    Instance.locationStates.ApplyDefaultState();
                }
            };
            Instance.nextLocationTimer.Restart(ScreenFade.FadeTime);
        }
        private IEnumerator WaitForSmoothLocationChange()
        {
            yield return new WaitForSeconds(ScreenFade.FadeTime * 2);
            OnLocationLoaded?.Invoke(GameData.Data.LocationsData.CurrentLocationId);
            SavingUtils.Instance.SaveGameData();
        }
        private void SetLocation(StateChange s)
        {
            GameData.Data.LocationsData.CurrentLocationId = locationStates.CurrentStateId;
            TryResetPlayerPosition(out _);
        }
        public static bool TryResetPlayerPosition(out Vector3 newPosition)
        {
            newPosition = Vector3.zero;
            Transform position = Instance.CurrentLocation.DefaultPosition;
            if (position == null)
            {
                InfoRequest.GetErrorRequest(100).Send();
                return false;
            }
            newPosition = position.position;
            Instance.playerInput.Moving.TeleportToIgnoreLayer(newPosition, Physics.AllLayers);
            return true;
        }
        private void LockInput()
        {
            if (isInputLocked) return;
            InputController.LockFullInput(int.MaxValue);
            isInputLocked = true;
        }
        private void UnlockInput()
        {
            if (!isInputLocked) return;
            InputController.UnlockFullInput(int.MaxValue);
            isInputLocked = false;
        }
        #endregion methods
    }
}