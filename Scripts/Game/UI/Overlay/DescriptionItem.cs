using Game.UI.Collections;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay
{
    public class DescriptionItem<T> : ContextActionsItem<T> where T : class
    {
        #region fields & properties
        [SerializeField] private CustomButton activationButton;
        [SerializeField] private GameObject activationIndicator;
        [SerializeField] private CustomButton deactivationButton;
        public bool IsSelected => isSelected;
        private bool isSelected = false;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            activationButton.OnClicked += SendPanelRequest;
            if (deactivationButton != null)
                deactivationButton.OnClicked += SendNullRequest;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            activationButton.OnClicked -= SendPanelRequest;
            if (deactivationButton != null)
                deactivationButton.OnClicked -= SendNullRequest;
        }
        protected virtual void SendPanelRequest()
        {
            new DescriptionPanelRequest<T>(this).Send();
        }
        protected virtual void SendNullRequest()
        {
            new DescriptionPanelRequest<T>(null).Send();
        }
        public virtual void SetItemActive(bool active)
        {
            isSelected = active;
            activationButton.enabled = !active;
            if (deactivationButton != null)
                deactivationButton.enabled = active;
            if (activationIndicator != null)
                activationIndicator.SetActive(active);
        }
        #endregion methods
    }
}