using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Universal.Behaviour
{
    [System.Serializable]
    public abstract class Observer : IDisposable, IInitializable
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public abstract void Dispose();

        public abstract void Initialize();
        #endregion methods
    }
}