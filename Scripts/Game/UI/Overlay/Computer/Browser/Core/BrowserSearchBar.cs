using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser
{
    public class BrowserSearchBar : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private VirtualBrowser browser;
        [SerializeField] private CustomButton button;
        [SerializeField] private TMP_InputField inputField;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            button.OnClicked += DoSearch;
            browser.OnFocusPageChanged += UpdateField;
            browser.OnCurrentStateChanged += OnBrowserStateChanged;
            if (browser.CurrentPage != null)
                UpdateField(browser.CurrentPage);
        }
        private void OnDisable()
        {
            button.OnClicked -= DoSearch;
            browser.OnFocusPageChanged -= UpdateField;
            browser.OnCurrentStateChanged -= OnBrowserStateChanged;
        }
        private void OnBrowserStateChanged(VirtualApplication.ApplicationState state)
        {
            if (state == VirtualApplication.ApplicationState.Closed)
                ResetField();
        }
        private void UpdateField(VirtualBrowserPage newPage)
        {
            inputField.text = browser.GetPageAddress(newPage);
        }
        private void ResetField()
        {
            UpdateField(browser.HomePage);
        }
        public void DoSearch()
        {
            browser.GoToPageByAddress(inputField.text);
        }
        #endregion methods
    }
}