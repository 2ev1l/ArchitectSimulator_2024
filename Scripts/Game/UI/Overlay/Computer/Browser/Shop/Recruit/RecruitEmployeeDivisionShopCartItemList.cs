using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitEmployeeDivisionShopCartItemList<Recruit, Employee> : RecruitEmployeeShopCartItemList<Recruit, Employee>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties
        protected abstract IReadOnlyDivision Division { get; }
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            Division.OnEmployeeHired += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Division.OnEmployeeHired -= UpdateListData;
        }
        private void UpdateListData(IEmployee _) => UpdateListData();
        #endregion methods
    }
}