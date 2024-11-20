using EditorCustom.Attributes;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class EnvironmentDataActions : MonoBehaviour
    {
        #region fields & properties
        public EnvironmentData Context => GameData.Data.EnvironmentData;
        #endregion fields & properties

        #region methods
        [SerializedMethod]
        public void ChangeLastTransportType(int transportType) => Context.LastTransportUsed = (TransportType)transportType;
        #endregion methods
    }
}