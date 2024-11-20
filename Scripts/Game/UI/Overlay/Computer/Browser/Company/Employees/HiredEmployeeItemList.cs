using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public abstract class HiredEmployeeItemList<Item, Employee> : InfinityFilteredItemListBase<Item, Employee>
        where Item : HiredEmployeeItem<Employee>
        where Employee : EmployeeData
    {
        #region fields & properties
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.CompanyData.OfficeData.OnEmployeeFired += TryUpdateListData;
            GameData.Data.CompanyData.OfficeData.OnEmployeeHired += TryUpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.CompanyData.OfficeData.OnEmployeeFired -= TryUpdateListData;
            GameData.Data.CompanyData.OfficeData.OnEmployeeHired -= TryUpdateListData;
        }
        #endregion fields & properties

        #region methods
        private void TryUpdateListData(EmployeeData employee)
        {
            if (employee is not Employee) return;
            UpdateListData();
        }
        protected override void UpdateCurrentItems(List<Employee> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<IEmployee> employees = GetDivision(GameData.Data.CompanyData.OfficeData.Divisions).Employees;
            int count = employees.Count;
            for (int i = 0; i < count; ++i)
            {
                currentItemsReference.Add((Employee)employees[i]);
            }
        }
        protected abstract IReadOnlyDivision GetDivision(DivisionsData divisions);
        #endregion methods
    }
}