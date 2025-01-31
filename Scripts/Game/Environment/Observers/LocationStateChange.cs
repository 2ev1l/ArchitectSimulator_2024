using Game.UI.Overlay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Environment.Observers
{
    public class LocationStateChange : PanelStateChange
    {
        #region fields & properties
        /// <summary>
        /// Returns first active object
        /// </summary>
        public Transform DefaultPosition => defaultPosition.Find(x => x.gameObject.activeInHierarchy);
        [SerializeField] private List<Transform> defaultPosition;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}