using EditorCustom.Attributes;
using Game.Events;
using Game.Serialization;
using Game.Serialization.World;
using Game.UI.Overlay;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Universal.Behaviour;
using Universal.Core;
using Universal.Events;
using Universal.Serialization;
using Universal.Time;
using Zenject;

namespace Game.Environment.Observers
{
    [System.Serializable]
    public class MonthObserver : Observer
    {
        #region fields & properties
        private static MonthObserver Instance;
        [SerializeField] private UIStateChange visibleCursorState;
        [SerializeField] private CanvasAlphaChanger canvasAlphaChanger;
        [SerializeField] private DefaultStateMachine panelsStates;
        [SerializeField] private MonthInfoPanel infoPanel;
        [SerializeField] private GameOverPanel gameOverPanel;
        [SerializeField] private GameOverChecker gameOverChecker;
        [SerializeField] private PopupRequestExecutor monthPopups;
        private bool isInputLocked = false;
        #endregion fields & properties

        #region methods
        public override void Dispose()
        {
            GameData.Data.PlayerData.MonthData.OnMonthChanged -= OnMonthChanged;
            GameData.Data.PlayerData.MonthData.OnBeforeMonthChanged -= OnBeforeMonthChanged;
            UnlockInput();
        }

        public override void Initialize()
        {
            Instance = this;
            HideCanvas();
            GameData.Data.PlayerData.MonthData.OnMonthChanged += OnMonthChanged;
            GameData.Data.PlayerData.MonthData.OnBeforeMonthChanged += OnBeforeMonthChanged;
        }

        /// <summary>
        /// Hides immediately
        /// </summary>
        public static void HideCanvas()
        {
            Instance.canvasAlphaChanger.HideCanvas();
            UnlockInput();
        }
        private void ShowCanvasSmoothly()
        {
            canvasAlphaChanger.FadeUp();
            LockInput();
        }

        public void OnBeforeMonthChanged()
        {
            RequestController.EnableExecution(monthPopups);
        }

        public void OnMonthChanged(int currentMonth)
        {
            RequestController.DisableExecution(monthPopups);
            UpdateUI();
        }

        private void UpdateUI()
        {
            ShowCanvasSmoothly();

            if (gameOverChecker.GetResult())
            {
                panelsStates.ApplyState(gameOverPanel);
                gameOverPanel.UpdateUI(gameOverChecker.LastCheckEndReason);
                SavingController.Instance.ResetTotalProgress();
            }
            else
            {
                panelsStates.ApplyState(infoPanel);
                infoPanel.UpdateUI();
                SavingController.Instance.SaveGameData();
            }
        }

        private static void LockInput()
        {
            if (Instance.isInputLocked) return;
            Instance.isInputLocked = true;
            InputController.LockFullInput(int.MaxValue);
            UIStateMachine.Instance.ApplyState(Instance.visibleCursorState);
        }
        private static void UnlockInput()
        {
            if (!Instance.isInputLocked) return;
            Instance.isInputLocked = false;
            InputController.UnlockFullInput(int.MaxValue);
            UIStateMachine.Instance.ApplyDefaultState();
        }
        #endregion methods
    }
}