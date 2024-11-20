using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class PremiseData : ChangableInfoData<PremiseInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected PremiseData(int id) : base(id) { }
        #endregion methods
    }
}