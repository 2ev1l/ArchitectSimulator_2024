using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Events;

namespace Game.UI.Overlay
{
    public abstract class OverlayPanelBase : MonoBehaviour, IRequestExecutor
    {
        #region fields & properties
        public UnityAction OnPanelClosed;
        public GameObject Panel => panel;
        [SerializeField] private GameObject panel;
        #endregion fields & properties

        #region methods
        public abstract bool TryExecuteRequest(ExecutableRequest request);
        #endregion methods
    }
}