using Game.Events;
using Game.Serialization.World;
using Game.UI.Elements;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitEmployeeShopCartItem<Recruit, Employee> : VirtualShopCartSingleItem<Recruit>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override bool CanBuy()
        {
            if (!GameData.Data.CompanyData.OfficeData.CanHireEmployee()) return false;
            return base.CanBuy();
        }
        #endregion methods
    }
}