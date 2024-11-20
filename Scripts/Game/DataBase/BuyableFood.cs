using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class BuyableFood : BuyableObject
    {
        #region fields & properties
        public override DBScriptableObjectBase ObjectReference => foodInfo;
        public FoodInfo Info => foodInfo.Data;
        [SerializeField] private FoodInfoSO foodInfo;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}