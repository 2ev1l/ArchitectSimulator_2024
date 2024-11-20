using Game.Serialization.Settings;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Serialization;

namespace Game.UI.Settings
{
    public class GameplayPanel : MonoBehaviour
    {
        #region fields & properties
        private GameplaySettings Context => SettingsData.Data.GameplaySettings;
        [SerializeField] private CustomCheckbox disableDeathCheckbox;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            disableDeathCheckbox.OnStateChanged += UpdateDeathTurn;
            ApplySettings();
        }
        private void OnDisable()
        {
            disableDeathCheckbox.OnStateChanged -= UpdateDeathTurn;
        }

        private void UpdateDeathTurn(bool state)
        {
            Context.DisableDeath = state;
        }
        private void ApplySettings()
        {
            disableDeathCheckbox.CurrentState = Context.DisableDeath;
        }
        #endregion methods
    }
}