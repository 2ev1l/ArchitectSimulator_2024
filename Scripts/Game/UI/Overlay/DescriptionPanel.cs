using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Universal.Events;
using Zenject;

namespace Game.UI.Overlay
{
    public abstract class DescriptionPanel<T> : RequestExecutorBehaviour where T : class
    {
        #region fields & properties
        /// <exception cref="System.NullReferenceException"></exception>
        protected T Data => data;
        private T data = null;
        protected DescriptionItem<T> ActiveItem => activeItem;
        private DescriptionItem<T> activeItem = null;
        public GameObject Panel => panel;
        [SerializeField] private GameObject panel;
        [SerializeField] private bool updateOnEnable = true;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            if (updateOnEnable)
                UpdateUI(activeItem);
        }
        protected virtual bool CanExecuteRequest(ExecutableRequest request, out DescriptionPanelRequest<T> descriptionRequest)
        {
            descriptionRequest = null;
            if (request is not DescriptionPanelRequest<T> dr) return false;
            descriptionRequest = dr;
            return true;
        }
        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (!CanExecuteRequest(request, out DescriptionPanelRequest<T> descriptionRequest)) return false;
            UpdateUI(descriptionRequest.Item);
            request.Close();
            return true;
        }
        private void UpdateUI(DescriptionItem<T> item)
        {
            if (activeItem != null)
                activeItem.SetItemActive(false);
            activeItem = item;
            if (activeItem == null)
            {
                data = null;
                UpdateNullUI();
                return;
            }
            panel.SetActive(true);
            activeItem.SetItemActive(true);
            UpdateContextUI(item.Context);
        }
        public void UpdateContextUI(T context)
        {
            this.data = context;
            if (data == null)
            {
                UpdateNullUI();
                return;
            }
            OnUpdateUI();
        }
        private void UpdateNullUI()
        {
            panel.SetActive(false);
            OnDataNull();
        }
        /// <summary>
        /// Guarantees that data will not be null
        /// </summary>
        protected abstract void OnUpdateUI();
        protected virtual void OnDataNull()
        {

        }
        private void OnValidate()
        {
            if (panel == gameObject)
                Debug.LogError("Component must be out of the panel");
        }
        #endregion methods
    }
}