using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public abstract class HiredSingleEmployeeItemList<Item, Employee> : SingleItemListBase<Item, Employee>
        where Item : HiredEmployeeItem<Employee>
        where Employee : EmployeeData
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.CompanyData.OfficeData.OnEmployeeFired += TryUpdateData;
            GameData.Data.CompanyData.OfficeData.OnEmployeeHired += TryUpdateData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.CompanyData.OfficeData.OnEmployeeFired -= TryUpdateData;
            GameData.Data.CompanyData.OfficeData.OnEmployeeHired -= TryUpdateData;
        }
        private void TryUpdateData(EmployeeData employee)
        {
            if (employee is not Employee) return;
            UpdateListData();
        }
        protected override Employee GetData()
        {
            return (Employee)GetRole(GameData.Data.CompanyData.OfficeData.Divisions).Employee;
        }
        protected abstract IReadOnlyRole GetRole(DivisionsData divisions);
        #endregion methods
    }
}