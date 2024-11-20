using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class HouseInfo : RealEstateInfo
    {
        #region fields & properties
        public int MaxPeople => maxPeople;
        [SerializeField][Range(1, 4)] private int maxPeople = 1;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}