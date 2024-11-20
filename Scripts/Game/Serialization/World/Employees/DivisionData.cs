using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class DivisionData<T> : IReadOnlyDivision where T : EmployeeData, ICloneable<T>
    {
        #region fields & properties
        public UnityAction<IEmployee> OnEmployeeHired { get; set; }
        public UnityAction<IEmployee> OnEmployeeFired { get; set; }

        public IReadOnlyList<IEmployee> Employees => employees;
        [SerializeField] private List<T> employees = new();
        #endregion fields & properties

        #region methods
        public void HireEmployee(T newEmployee)
        {
            T emp = newEmployee.Clone();
            emp.ChangeId(GetFreeId());
            employees.Add(emp);
            OnEmployeeHired?.Invoke(newEmployee);
        }
        public void FireEmployee(T oldEmployee)
        {
            employees.Remove(oldEmployee);
            OnEmployeeFired?.Invoke(oldEmployee);
        }
        public int GetFreeId()
        {
            if (employees.Count == 0) return 0;
            return employees[GetEmployeesCount() - 1].Id + 1;
        }
        public int GetEmployeesCount()
        {
            return employees.Count;
        }
        public int GetEmployeesSalary()
        {
            IReadOnlyList<IEmployee> employees = Employees;
            int employeesCount = employees.Count;
            int sum = 0;
            for (int i = 0; i < employeesCount; ++i)
            {
                sum += employees[i].Salary;
            }
            return sum;
        }
        #endregion methods
    }
}