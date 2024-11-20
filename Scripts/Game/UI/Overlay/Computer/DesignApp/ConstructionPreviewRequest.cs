using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Overlay.Computer.DesignApp
{
    [System.Serializable]
    public class ConstructionPreviewRequest : ExecutableRequest
    {
        #region fields & properties
        public BlueprintBaseData BlueprintBaseData => blueprintBaseData;
        private BlueprintBaseData blueprintBaseData;
        #endregion fields & properties

        #region methods
        public override void Close() { }
        public ConstructionPreviewRequest(BlueprintBaseData blueprintBaseData)
        {
            this.blueprintBaseData = blueprintBaseData;
        }
        #endregion methods

    }
}