using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    //Not much different employees, so departments isn't required
    [System.Serializable]
    public class DivisionsData : IBillPayable
    {
        #region fields & properties
        public int BillPaymentAmount => GetTotalEmployeesSalary();
        public bool CanAddBill => GetEmployeesCount() > 0;
        public string BillDescription => LanguageInfo.GetTextByType(TextType.Game, 192);

        public IReadOnlyDivision Builders => builders;
        internal DivisionData<BuilderData> BuildersInternal => builders;
        [SerializeField] private DivisionData<BuilderData> builders = new();

        public IReadOnlyRole HRManager => hrManager;
        internal RoleData<HRManagerData> HRManagerInternal => hrManager;
        [SerializeField] private RoleData<HRManagerData> hrManager = new();

        public IReadOnlyRole PRManager => prManager;
        internal RoleData<PRManagerData> PRManagerInternal => prManager;
        [SerializeField] private RoleData<PRManagerData> prManager = new();
        public IReadOnlyRole DesignEngineer => designEngineer;
        internal DesignEngineerRoleData DesignEngineerInternal => designEngineer;
        [SerializeField] private DesignEngineerRoleData designEngineer = new();
        #endregion fields & properties

        #region methods
        public int GetTotalEmployeesSalary()
        {
            int sum = 0;
            foreach (var employee in GetNotNullSingleEmployees())
            {
                sum += employee.Salary;
            }
            foreach (var e in builders.Employees)
            {
                BuilderData builder = (BuilderData)e;
                if (builder.IsBusy)
                {
                    sum += builder.Salary;
                }
            }
            sum = Mathf.Max(sum, 0);
            return sum;
        }
        private List<ISingleEmployee> GetNotNullSingleEmployees()
        {
            List<ISingleEmployee> result = new();
            void TryAddToResult<T>(RoleData<T> role) where T : EmployeeData, ICloneable<T>, ISingleEmployee
            {
                if (role.IsEmployeeHired())
                    result.Add(role.Employee);
            }
            TryAddToResult(hrManager);
            TryAddToResult(prManager);
            TryAddToResult(designEngineer);
            return result;
        }

        public int GetEmployeesCount()
        {
            int count = GetNotNullSingleEmployees().Count;
            count += builders.GetEmployeesCount();
            return count;
        }
        #endregion methods
    }
}