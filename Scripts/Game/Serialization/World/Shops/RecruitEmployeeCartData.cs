using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RecruitEmployeeCartData<Recruit, Employee> : SingleItemCartData<Recruit>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool CanPurchaseCart()
        {
            if (!GameData.Data.CompanyData.OfficeData.CanHireEmployee()) return false;
            return base.CanPurchaseCart();
        }
        #endregion methods
    }
}