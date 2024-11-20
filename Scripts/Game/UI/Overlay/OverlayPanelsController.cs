using EditorCustom.Attributes;
using Game.UI.Elements;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;
using Universal.Events;
using System.Linq;
using Game.Events;
using UnityEngine.Events;
using Universal.Behaviour;
using Zenject;

namespace Game.UI.Overlay
{
    [System.Serializable]
    public class OverlayPanelsController : RequestExecutor
    {
        #region fields & properties
        public UnityAction OnPanelOpened;
        public UnityAction OnPanelClosed;
        [SerializeField] private GameObject raycastBlock;
        [SerializeField] private List<OverlayPanelBase> overlayPanels;
        public bool IsPanelOpened => isPanelOpened;
        [SerializeField][ReadOnly] private bool isPanelOpened = false;
        [Inject] private InputController inputController;
        #endregion fields & properties

        #region methods
        public override void Dispose()
        {
            base.Dispose();
            overlayPanels.ForEach(x => x.OnPanelClosed -= EnableInput);
        }
        public override void Initialize()
        {
            base.Initialize();
            overlayPanels.ForEach(x => x.OnPanelClosed += EnableInput);
        }
        private void EnableInput()
        {
            raycastBlock.SetActive(false);
            InputController.UnlockFullInput(int.MaxValue);
            isPanelOpened = false;
            OnPanelClosed?.Invoke();
        }
        private void DisableInput()
        {
            raycastBlock.SetActive(true);
            InputController.LockFullInput(int.MaxValue);
            isPanelOpened = true;
            OnPanelOpened?.Invoke();
        }
        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (IsPanelOpened) return false;
            bool isExecuted = false;
            foreach (var overlayPanel in overlayPanels)
            {
                if (overlayPanel.TryExecuteRequest(request))
                    isExecuted = true;
            }
            if (isExecuted)
                DisableInput();
            return isExecuted;
        }
        #endregion methods
    }
}