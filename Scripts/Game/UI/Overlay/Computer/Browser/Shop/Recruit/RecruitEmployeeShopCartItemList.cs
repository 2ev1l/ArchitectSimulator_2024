using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitEmployeeShopCartItemList<Recruit, Employee> : ConstantShopItemsList<RecruitEmployeeShopCartItem<Recruit, Employee>, Recruit>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}