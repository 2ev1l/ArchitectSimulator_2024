using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Collections
{
    [System.Serializable]
    public class DeselectRequest<T> : ExecutableRequest where T : class
    {
        #region fields & properties
        public T Deselected => deselected;
        private T deselected;
        #endregion fields & properties

        #region methods
        public override void Close() { }
        public DeselectRequest(T deselected)
        {
            this.deselected = deselected;
        }
        #endregion methods
    }
}