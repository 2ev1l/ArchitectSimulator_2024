using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class HiredBuilderItemList : HiredEmployeeItemList<HiredBuilderItem, BuilderData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override IReadOnlyDivision GetDivision(DivisionsData divisions)
        {
            return divisions.Builders;
        }
        #endregion methods
    }
}