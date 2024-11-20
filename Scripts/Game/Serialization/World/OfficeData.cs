using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class OfficeData : RentablePremiseData
    {
        #region fields & properties
        public UnityAction<EmployeeData> OnEmployeeHired;
        public UnityAction<EmployeeData> OnEmployeeFired;
        public override string BillDescription => LanguageInfo.GetTextByType(TextType.Game, 190);
        public DivisionsData Divisions => divisions;
        [SerializeField] private DivisionsData divisions = new();
        #endregion fields & properties

        #region methods
        protected override RentablePremise GetRentablePremiseInfo() => DB.Instance.RentableOfficeInfo.Find(x => x.Data.PremiseInfo.Id == Id).Data;
        public override bool CanReplaceInfo(int newInfoId)
        {
            if (!base.CanReplaceInfo(newInfoId)) return false;
            if (newInfoId == -1) return divisions.GetEmployeesCount() == 0;
            OfficeInfo newOffice = (OfficeInfo)DB.Instance.RentableOfficeInfo.Find(x => x.Data.PremiseInfo.Id == newInfoId).Data.PremiseInfo;
            return (newOffice.MaximumEmployees - divisions.GetEmployeesCount()) >= 0;
        }
        public bool CanHireEmployee()
        {
            if (Id < 0) return false;
            if (GetFreeEmployeesSpace() <= 0) return false;
            return true;
        }
        public int GetFreeEmployeesSpace() => ((OfficeInfo)Info).MaximumEmployees - divisions.GetEmployeesCount();

        public void FireDesignEngineer() => FireSingleEmployee(divisions.DesignEngineerInternal);
        public bool TryHireDesignEngineer(DesignEngineerData newDE) => TryHireSingleEmployee(divisions.DesignEngineerInternal, newDE);

        public void FirePRManager() => FireSingleEmployee(divisions.PRManagerInternal);
        public bool TryHirePRManager(PRManagerData newPR) => TryHireSingleEmployee(divisions.PRManagerInternal, newPR);

        public void FireHRManager() => FireSingleEmployee(divisions.HRManagerInternal);
        public bool TryHireHRManager(HRManagerData newHR) => TryHireSingleEmployee(divisions.HRManagerInternal, newHR);

        public bool TryHireBuilder(BuilderData builder) => TryHireEmployee(divisions.BuildersInternal, builder);
        public void FireBuilder(BuilderData builder) => FireEmployee(divisions.BuildersInternal, builder);
        private bool TryHireSingleEmployee<T>(RoleData<T> division, T employee) where T : EmployeeData, ICloneable<T>, ISingleEmployee
        {
            if (!CanHireEmployee()) return false;
            if (division.IsEmployeeHired()) return false;
            division.ReplaceEmployee(employee);
            OnEmployeeHired?.Invoke(employee);
            return true;
        }
        private void FireSingleEmployee<T>(RoleData<T> division) where T : EmployeeData, ICloneable<T>, ISingleEmployee
        {
            EmployeeData empl = division.Employee as EmployeeData;
            division.FireEmployee();
            OnEmployeeFired?.Invoke(empl);
        }
        private bool TryHireEmployee<T>(DivisionData<T> division, T employee) where T : EmployeeData, ICloneable<T>
        {
            if (!CanHireEmployee()) return false;
            division.HireEmployee(employee);
            OnEmployeeHired?.Invoke(employee);
            return true;
        }
        private void FireEmployee<T>(DivisionData<T> division, T employee) where T : EmployeeData, ICloneable<T>
        {
            division.FireEmployee(employee);
            OnEmployeeFired?.Invoke(employee);
        }
        public OfficeData(int id) : base(id) { }
        #endregion methods
    }
}