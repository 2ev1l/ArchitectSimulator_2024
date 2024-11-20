using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Overlay.Computer.Collections
{
    public abstract class ResourceDataPanelRequestExecutor<ResourceData> : RequestExecutorBehaviour
        where ResourceData : Game.Serialization.World.ResourceData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is not ResourceDataPanelRequest<ResourceData> req) return false;
            if (req.DoOpen)
                OpenPanel(req);
            else
                ClosePanel();
            return true;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ClosePanel();
        }
        protected abstract void OpenPanel(ResourceDataPanelRequest<ResourceData> request);
        protected abstract void ClosePanel();
        #endregion methods
    }
}