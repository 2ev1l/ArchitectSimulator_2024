using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionResourcesWarehouseData : ResourcesWarehouseData<ConstructionResourceData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ConstructionResourceData GetNewResource(int id) => new(id);
        #endregion methods
    }
}