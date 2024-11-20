using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Universal.Events
{
    [System.Serializable]
    public static class RequestController
    {
        #region fields & properties
        private static readonly HashSet<IRequestExecutor> executors = new();
        #endregion fields & properties

        #region methods
        public static void DisableExecution(IRequestExecutor obj) => executors.Remove(obj);
        public static void EnableExecution(IRequestExecutor obj) => executors.Add(obj);

        public static bool TryExecuteRequest(ExecutableRequest request)
        {
            bool executed = false;
            foreach (IRequestExecutor exec in executors)
            {
                if (exec.TryExecuteRequest(request))
                    executed = true;
            }
            return executed;
        }
        #endregion methods
    }
}