using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RecruitSingleEmployeeCartData<Recruit, Employee> : RecruitEmployeeCartData<Recruit, Employee>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData, ISingleEmployee
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool CanPurchaseCart()
        {
            IReadOnlyRole role = GetRole(GameData.Data.CompanyData.OfficeData.Divisions);
            if (role.IsEmployeeHired()) return false;
            return base.CanPurchaseCart();
        }
        public abstract IReadOnlyRole GetRole(DivisionsData divisions);
        #endregion methods
    }
}