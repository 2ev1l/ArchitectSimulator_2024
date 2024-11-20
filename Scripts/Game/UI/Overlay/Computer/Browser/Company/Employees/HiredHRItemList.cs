using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class HiredHRItemList : HiredSingleEmployeeItemList<HiredHRItem, HRManagerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override IReadOnlyRole GetRole(DivisionsData divisions)
        {
            return divisions.HRManager;
        }
        #endregion methods
    }
}