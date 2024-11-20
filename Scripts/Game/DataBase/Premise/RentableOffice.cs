using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class RentableOffice : RentablePremise
    {
        #region fields & properties
        public override DBScriptableObjectBase ObjectReference => info;
        public override PremiseInfo PremiseInfo => info.Data;
        [SerializeField] private OfficeInfoSO info;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}