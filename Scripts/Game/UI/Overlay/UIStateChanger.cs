using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay
{
    public class UIStateChanger : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private int stateId;
        #endregion fields & properties

        #region methods
        [SerializedMethod]
        public void Change() => ChangeTo(stateId);
        [SerializedMethod]
        public void ChangeTo(int stateId) => UIStateMachine.Instance.ApplyState(stateId);
        #endregion methods
    }
}