using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Overlay.Computer.Collections
{
    [System.Serializable]
    public class ResourceDataPanelRequest<ResourceData> : ExecutableRequest
        where ResourceData : Game.Serialization.World.ResourceData
    {
        #region fields & properties
        public ResourceData Data { get; }
        public bool DoOpen { get; }
        #endregion fields & properties

        #region methods
        public override void Close()
        {

        }
        public ResourceDataPanelRequest(ResourceData resourceData, bool doOpen)
        {
            this.Data = resourceData;
            this.DoOpen = doOpen;
        }
        #endregion methods
    }
}