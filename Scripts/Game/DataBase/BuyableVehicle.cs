using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class BuyableVehicle : BuyableObject
    {
        #region fields & properties
        public override DBScriptableObjectBase ObjectReference => vehicleInfo;
        public VehicleInfo Info => vehicleInfo.Data;
        [SerializeField] private VehicleInfoSO vehicleInfo;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}