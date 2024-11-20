using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitEmployeeShopCartBehaviour<Recruit, Employee> : VirtualShopCartBehaviour<Recruit>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}