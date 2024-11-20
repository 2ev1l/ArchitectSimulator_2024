using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Environment.Observers
{
    public class ObserversActions : MonoBehaviour
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        [SerializedMethod]
        public void HideMonthCanvas() => MonthObserver.HideCanvas();
        [SerializedMethod]
        public void MoveToLocation(int locationId) => LocationObserver.MoveToLocation(locationId);
        [SerializedMethod]
        public void TryResetPlayerPosition() => LocationObserver.TryResetPlayerPosition(out _);
        #endregion methods
    }
}