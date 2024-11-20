using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class HiredDesignEngineerItemList : HiredSingleEmployeeItemList<HiredDesignEngineerItem, DesignEngineerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override IReadOnlyRole GetRole(DivisionsData divisions)
        {
            return divisions.DesignEngineer;
        }
        #endregion methods
    }
}