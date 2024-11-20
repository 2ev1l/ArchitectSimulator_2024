using EditorCustom.Attributes;
using Game.Cameras;
using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Overlay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Behaviour;
using Universal.Core;
using Universal.Time;
using Zenject;

namespace Game.Environment
{
    public class OfficeStateMachine : MonoBehaviour
    {
        #region fields & properties
        private OfficeStateChange CurrentState => (OfficeStateChange)stateMachine.CurrentState;
        [SerializeField] private StateMachine stateMachine = new();
        [SerializeField] private Transform officeEntry;
        [SerializeField] private CinemachineCamerasController cameraController;
        private bool isInputLocked = false;
        private Timer enterTimer = new();
        [Inject] private ScreenFade screenFade;
        [Inject] private Player.Input playerInput;
        #endregion fields & properties

        #region methods
        [SerializedMethod]
        public void Enter()
        {
            int officeId = GameData.Data.CompanyData.OfficeData.Id;
            if (!stateMachine.States.Exists(x => ((OfficeStateChange)x).OfficeId == officeId, out StateChange exist))
            {
                InfoRequest.GetErrorRequest(101).Send();
                return;
            }

            LockInput();
            screenFade.DoCycle();
            enterTimer.OnChangeEnd = delegate
            {
                EnterImmediately(exist);
                UnlockInput();
            };
            enterTimer.Restart(ScreenFade.FadeTime);
        }
        private void EnterImmediately(StateChange officeState)
        {
            stateMachine.TryApplyState(officeState);
            playerInput.Moving.TeleportToIgnoreLayer(CurrentState.SafePosition.position, Physics.AllLayers);
        }
        [SerializedMethod]
        public void Exit()
        {
            LockInput();
            screenFade.DoCycle();
            enterTimer.OnChangeEnd = delegate
            {
                ExitImmediately();
                UnlockInput();
            };
            enterTimer.Restart(ScreenFade.FadeTime);
        }
        private void ExitImmediately()
        {
            playerInput.Moving.TeleportToIgnoreLayer(officeEntry.position, Physics.AllLayers);
        }
        private void LockInput()
        {
            if (isInputLocked) return;
            isInputLocked = true;
            InputController.LockFullInput(int.MaxValue);
            cameraController.DisableCamerasSpeed();
        }
        private void UnlockInput()
        {
            if (!isInputLocked) return;
            isInputLocked = false;
            InputController.UnlockFullInput(int.MaxValue);
            cameraController.EnableCamerasSpeed();
        }
        #endregion methods

#if UNITY_EDITOR
        [Button(nameof(GetAllStatesInParent))]
        private void GetAllStatesInParent()
        {
            List<StateChange> states = transform.parent.GetComponentsInChildren<OfficeStateChange>().Select(x => (StateChange)x).ToList();
            UnityEditor.Undo.RecordObject(this, "Set states");
            stateMachine.ReplaceStates(states);
        }
        [Button(nameof(DebugStates))]
        private void DebugStates()
        {
            HashSet<StateChange> sameStates = stateMachine.States.FindSame((x, y) => ((OfficeStateChange)x).OfficeId == ((OfficeStateChange)y).OfficeId);
            if (sameStates.Count > 0)
            {
                foreach (OfficeStateChange el in sameStates.Cast<OfficeStateChange>())
                {
                    LogOffice($"Same id #{el.OfficeId}", el);
                }
            }
            List<int> offices = DB.Instance.OfficeInfo.Data.Select(x => x.Id).ToList();
            foreach (OfficeStateChange state in stateMachine.States.Cast<OfficeStateChange>())
            {
                offices.Remove(state.OfficeId);
                if (state.SafePosition == null)
                    LogOffice($"Safe position is null", state);
            }
            foreach (int el in offices)
            {
                Debug.LogError($"Can't find id #{el} for office");
            }
        }

        private void LogOffice(string log, OfficeStateChange state) => Debug.LogError($"{state.name} : <color=#FFCCCC>{log}</color>", state); 
#endif //UNITY_EDITOR
    }
}