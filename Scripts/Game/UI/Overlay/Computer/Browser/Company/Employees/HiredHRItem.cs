using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class HiredHRItem : HiredEmployeeItem<HRManagerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void FireEmployee(OfficeData office)
        {
            office.FireHRManager();
        }
        #endregion methods
    }
}