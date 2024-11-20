using Game.Events;
using UnityEngine;
using Universal.Collections;
using Universal.Collections.Generic;
using Universal.Events;

namespace Game.UI.Overlay
{
    public class PopupRequestExecutor : RequestExecutorBehaviour
    {
        #region fields & properties
        [SerializeField] private ObjectPool<DestroyablePoolableObject> popupPool;
        #endregion fields & properties

        #region methods
        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is not PopupRequest popupRequest) return false;
            PopupStatsContent popupContent = (PopupStatsContent)popupPool.GetObject();
            popupContent.UpdateUI(popupRequest);
            popupRequest.Close();
            return true;
        }
        #endregion methods

    }
}