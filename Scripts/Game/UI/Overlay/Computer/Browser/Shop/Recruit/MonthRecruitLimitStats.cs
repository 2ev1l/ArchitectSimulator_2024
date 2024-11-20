using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class MonthRecruitLimitStats : TextStatsContent
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            CompanyData.OfficeData.OnEmployeeHired += UpdateUI;
            CompanyData.OfficeData.OnEmployeeFired += UpdateUI;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            CompanyData.OfficeData.OnEmployeeHired -= UpdateUI;
            CompanyData.OfficeData.OnEmployeeFired -= UpdateUI;
        }
        private void UpdateUI(EmployeeData empl)
        {
            if (empl is not HRManagerData) return;
            UpdateUI();
        }
        public override void UpdateUI()
        {
            int hrSkill = -1;
            if (CompanyData.OfficeData.Divisions.HRManager.IsEmployeeHired())
            {
                hrSkill = CompanyData.OfficeData.Divisions.HRManager.Employee.SkillLevel;
            }
            int maxEmployees = GameData.Data.BrowserData.BuildersRecruit.GetMaxEmployees(hrSkill);
            if (maxEmployees > 1)
                Text.text = $"1x ~ {maxEmployees}x";
            else
                Text.text = $"1x";
        }
        #endregion methods
    }
}