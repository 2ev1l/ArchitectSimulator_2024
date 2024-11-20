using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Events
{
    public abstract class RequestExecutorBehaviour : MonoBehaviour, IRequestExecutor
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public abstract bool TryExecuteRequest(ExecutableRequest request);
        protected virtual void OnEnable()
        {
            RequestController.EnableExecution(this);
        }
        protected virtual void OnDisable()
        {
            RequestController.DisableExecution(this);
        }
        #endregion methods
    }
}