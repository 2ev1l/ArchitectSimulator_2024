using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI.Collections
{
    public class SelectableItem<T> : ContextActionsItem<T> where T : class
    {
        #region fields & properties
        [SerializeField] private GameObject selectedUI;
        [SerializeField] private CustomButton selectButton;
        [SerializeField] private CustomButton deselectButton;
        public bool IsSelected => IsSelected;
        private bool isSelected;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            selectButton.OnClicked += Select;
            deselectButton.OnClicked += Deselect;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            selectButton.OnClicked -= Select;
            deselectButton.OnClicked -= Deselect;
        }
        public void Select() => ChangeSelectState(true);
        public void Deselect() => ChangeSelectState(false);
        private void ChangeSelectState(bool isSelected)
        {
            if (this.isSelected == isSelected) return;
            this.isSelected = isSelected;
            if (selectedUI.activeSelf != isSelected)
                selectedUI.SetActive(isSelected);
            selectButton.enabled = !isSelected;
            deselectButton.enabled = isSelected;
            OnSelectStateChanged(isSelected);
            if (isSelected)
                new SelectRequest<T>(Context).Send();
            else
                new DeselectRequest<T>(Context).Send();
        }
        protected virtual void OnSelectStateChanged(bool isSelected) { }
        #endregion methods
    }
}