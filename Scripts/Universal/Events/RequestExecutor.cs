using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Universal.Events
{
    [System.Serializable]
    public abstract class RequestExecutor : IDisposable, IRequestExecutor, IInitializable
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        public abstract bool TryExecuteRequest(ExecutableRequest request);
        public virtual void Initialize()
        {
            RequestController.EnableExecution(this);
        }
        public virtual void Dispose()
        {
            RequestController.DisableExecution(this);
        }

        #endregion methods
    }
}