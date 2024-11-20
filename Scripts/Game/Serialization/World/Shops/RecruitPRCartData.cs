using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitPRCartData : RecruitSingleEmployeeCartData<RecruitPRData, PRManagerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override IReadOnlyRole GetRole(DivisionsData divisions) => divisions.PRManager;
        #endregion methods
    }
}