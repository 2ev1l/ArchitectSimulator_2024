using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitHRCartData : RecruitSingleEmployeeCartData<RecruitHRData, HRManagerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override IReadOnlyRole GetRole(DivisionsData divisions) => divisions.HRManager;
        #endregion methods

    }
}