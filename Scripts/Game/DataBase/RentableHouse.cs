using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class RentableHouse : RentableRealEstate
    {
        #region fields & properties
        public override RealEstateInfo RealEstateInfo => houseInfo.Data;
        public override DBScriptableObjectBase ObjectReference => houseInfo;
        [SerializeField] private HouseInfoSO houseInfo;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}