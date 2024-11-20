using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Collections
{
    [System.Serializable]
    public class SelectRequest<T> : ExecutableRequest where T : class
    {
        #region fields & properties
        public T Selected => selected;
        private T selected;
        #endregion fields & properties

        #region methods
        public override void Close() { }
        public SelectRequest(T selected)
        {
            this.selected = selected;
        }
        #endregion methods
    }
}