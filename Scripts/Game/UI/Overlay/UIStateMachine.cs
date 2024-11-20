using Game.Events;
using Game.Serialization.Settings.Input;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Game.UI.Overlay
{
    public class UIStateMachine : DefaultStateMachine
    {
        #region fields & properties
        public static UIStateMachine Instance => instance;
        private static UIStateMachine instance;
        private StateChange oldState = null;
        [SerializeField] private StateChange settingsState;
        [SerializeField] private List<StateChange> enablePlayerInputStates;
        [SerializeField] private List<StateChange> returnDefaultStates;
        private bool playerInputWasDisabled = false;
        private bool uiInputWasDisabled = false;
        #endregion fields & properties

        #region methods
        public void ForceInitialize()
        {
            instance = this;
        }
        protected override void OnEnable()
        {
            base.Context.OnStateChanged += CheckStateChanged;
            base.OnEnable();
            InputController.OnKeyDown += CheckDownKey;
            CheckStateChanged(Context.CurrentState);
        }
        protected override void OnDisable()
        {
            base.Context.OnStateChanged -= CheckStateChanged;
            base.OnDisable();
            InputController.OnKeyDown -= CheckDownKey;
        }
        private void OnDestroy()
        {
            EnablePlayerInput();
            EnableUIInput();
        }
        private void CheckStateChanged(StateChange newState)
        {
            if (!enablePlayerInputStates.Contains(newState))
                DisablePlayerInput();
            else
                EnablePlayerInput();

            if (newState == Context.DefaultState && oldState == settingsState)
                EnableUIInput();
            if (newState == settingsState)
                DisableUIInput();

            oldState = newState;
        }
        private void CheckDownKey(KeyCodeInfo info)
        {
            if (info.Description.Equals(KeyCodeDescription.OpenSettings))
            {
                if (returnDefaultStates.Contains(Context.CurrentState)) ApplyDefaultState();
                else ApplyState(settingsState);
                return;
            }
            if (info.Description.Equals(((UIStateChange)Context.CurrentState).CloseKey))
            {
                ApplyDefaultState();
                return;
            }

            ApplyStateByKeyCode(info.Description);
        }
        private void ApplyStateByKeyCode(KeyCodeDescription description)
        {
            foreach (UIStateChange state in base.Context.States.Cast<UIStateChange>())
            {
                if (!state.OpenKey.Equals(description)) continue;
                ApplyState(state);
                return;
            }
        }
        private void DisableUIInput()
        {
            if (uiInputWasDisabled) return;
            InputController.LockUIInput(1);
            uiInputWasDisabled = true;
        }
        private void EnableUIInput()
        {
            if (!uiInputWasDisabled) return;
            InputController.UnlockUIInput(1);
            uiInputWasDisabled = false;
        }

        private void DisablePlayerInput()
        {
            if (playerInputWasDisabled) return;
            InputController.LockPlayerInput(1);
            playerInputWasDisabled = true;
        }
        private void EnablePlayerInput()
        {
            if (!playerInputWasDisabled) return;
            InputController.UnlockPlayerInput(1);
            playerInputWasDisabled = false;
        }
        #endregion methods
    }
}