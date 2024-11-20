using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionResourceData : ResourceData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ResourceInfo GetInfo() => DB.Instance.ConstructionResourceInfo[Id].Data;
        public ConstructionResourceData(int id) : base(id) { }
        #endregion methods
    }
}