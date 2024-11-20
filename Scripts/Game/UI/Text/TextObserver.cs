using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.DataBase;
using Game.Serialization.Settings;
using System.Linq;
using Universal.Core;
using Universal.Serialization;
using Universal.Behaviour;

namespace Game.UI.Text
{
    [System.Serializable]
    public class TextObserver : Observer
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override void Dispose()
        {
            SettingsData.Data.OnLanguageChanged -= UpdateText;
            SceneLoader.OnSceneLoaded -= WaitForUpdateText;
        }

        public override void Initialize()
        {
            SettingsData.Data.OnLanguageChanged += UpdateText;
            SceneLoader.OnSceneLoaded += WaitForUpdateText;
            WaitForUpdateText();
        }
        private void WaitForUpdateText() => SingleGameInstance.Instance.StartCoroutine(WaitForUpdateText_IEnumerator());
        private IEnumerator WaitForUpdateText_IEnumerator()
        {
            yield return null;
            UpdateText();
        }
        private void UpdateText(LanguageSettings languageSettings) => UpdateText();
        private void UpdateText()
        {
            LoadChoosedLanguage();
            //can be made with interfaces
            foreach (var el in GameObject.FindObjectsByType<LanguageLoader>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                el.Load();
            }
            foreach (var el in GameObject.FindObjectsByType<TextUpdater>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                el.UpdateText();
            }
        }
        public void LoadChoosedLanguage()
        {
            try
            {
                TextData.LoadedData = LanguageData.GetLanguage(SettingsData.Data.LanguageSettings.ChoosedLanguage);
            }
            catch
            {
                Debug.LogError("Error - Can't find a language. Settting English by default.");
                SettingsData.Data.LanguageSettings.ResetLanguage();
            }
        }


        #endregion methods
    }
}