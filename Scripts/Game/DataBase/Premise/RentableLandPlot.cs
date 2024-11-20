using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class RentableLandPlot : RentablePremise
    {
        #region fields & properties
        public override PremiseInfo PremiseInfo => info.Data;
        public override DBScriptableObjectBase ObjectReference => info;
        [SerializeField] private LandPlotInfoSO info;
        #endregion fields & properties

        #region methods

        #endregion methods

    }
}