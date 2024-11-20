using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class ConstructionResourceGroup : ResourceGroup<ConstructionResourceInfoSO, ConstructionResourceInfo>
    {
        #region fields & properties
        public ConstructionResourceGroupType GroupTypes => groupTypes;
        [SerializeField] private ConstructionResourceGroupType groupTypes;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}