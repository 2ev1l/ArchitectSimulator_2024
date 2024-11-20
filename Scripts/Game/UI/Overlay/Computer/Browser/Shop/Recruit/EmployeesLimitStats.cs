using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class EmployeesLimitStats : TextStatsContent
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
        private void UpdateUI(EmployeeData _) => UpdateUI();
        public override void UpdateUI()
        {
            Text.text = $"{CompanyData.OfficeData.GetFreeEmployeesSpace()}x";
        }
        #endregion methods
    }
}