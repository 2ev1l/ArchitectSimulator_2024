using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RoleData<T> : IReadOnlyRole where T : EmployeeData, ICloneable<T>, ISingleEmployee
    {
        #region fields & properties
        public UnityAction<ISingleEmployee> OnHired { get; set; }
        public UnityAction<ISingleEmployee> OnFired { get; set; }

        /// <exception cref="System.NullReferenceException"></exception>
        public ISingleEmployee Employee => employee;
        private T employee => IsEmployeeHired() ? employees[0] : null;
        [SerializeField] private List<T> employees = new();
        #endregion fields & properties

        #region methods
        public bool IsEmployeeHired() => employees.Count > 0;
        public void ReplaceEmployee(T newEmployee)
        {
            FireEmployee();
            employees.Add(newEmployee.Clone());
            OnEmployeeHired(employee);
            OnHired?.Invoke(employee);
        }
        /// <summary>
        /// Invokes just before the action
        /// </summary>
        /// <param name="oldEmployee"></param>
        protected virtual void OnEmployeeHired(T newEmployee) { }
        public void FireEmployee()
        {
            if (!IsEmployeeHired()) return;
            T oldEmployee = employee;
            employees.RemoveAt(0);
            OnEmployeeFired(oldEmployee);
            OnFired?.Invoke(oldEmployee);
        }
        /// <summary>
        /// Invokes just before the action
        /// </summary>
        /// <param name="oldEmployee"></param>
        protected virtual void OnEmployeeFired(T oldEmployee) { }
        #endregion methods
    }
}