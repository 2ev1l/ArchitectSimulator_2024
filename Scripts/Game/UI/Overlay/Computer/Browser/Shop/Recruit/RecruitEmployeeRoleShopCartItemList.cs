using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitEmployeeRoleShopCartItemList<Recruit, Employee> : RecruitEmployeeShopCartItemList<Recruit, Employee>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData, ISingleEmployee
    {
        #region fields & properties
        protected abstract IReadOnlyRole Role { get; }
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            Role.OnHired += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Role.OnHired -= UpdateListData;
        }
        private void UpdateListData(IEmployee _) => UpdateListData();
        #endregion methods
    }
}