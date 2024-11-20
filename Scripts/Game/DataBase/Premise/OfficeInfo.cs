using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class OfficeInfo : PremiseInfo
    {
        #region fields & properties
        public int MaximumEmployees => maximumEmployees;
        [SerializeField][Min(1)] private int maximumEmployees = 1;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}