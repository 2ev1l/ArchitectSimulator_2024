using Game.UI.Overlay.Computer.Core;
using Game.UI.Overlay.Computer.DesignApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class BuildingConstructionApplication : VirtualApplication
    {
        #region fields & properties
        [SerializeField] private FixedUI fixedPanels;
        [SerializeField] private List<DefaultStateMachine> stateMachines;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            if (fixedPanels != null)
                fixedPanels.Block();
        }
        private void OnDisable()
        {
            if (fixedPanels != null)
                fixedPanels.Unlock();
        }
        public override void OnViewFocusChanged()
        {
            base.OnViewFocusChanged();
            if (IsMainVisibleApplication()) return;
            foreach (var el in stateMachines)
            {
                el.ApplyDefaultState();
            }
        }
        protected override void OnCloseApplication()
        {
            base.OnCloseApplication();
            foreach (var el in stateMachines)
            {
                el.ApplyDefaultState();
            }
        }
        #endregion methods
    }
}