using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Serialization
{
    [System.Serializable]
    public class GameplaySettings
    {
        #region fields & properties
        public bool DisableDeath
        {
            get => disableDeath;
            set => disableDeath = value;
        }
        [SerializeField] private bool disableDeath = false;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}