using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public abstract class HiredEmployeeItem<Employee> : ContextActionsItem<Employee>
        where Employee : EmployeeData
    {
        #region fields & properties
        [SerializeField] private EmployeeItem employeeItem;
        [SerializeField] private CustomButton fireButton;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            fireButton.OnClicked += RequestFireEmployee;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            fireButton.OnClicked -= RequestFireEmployee;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            fireButton.enabled = CanFire();
        }
        private void RequestFireEmployee()
        {
            if (!CanFire()) return;
            new ConfirmRequest(Fire, null, LanguageInfo.GetTextByType(TextType.Game, 105), LanguageInfo.GetTextByType(TextType.Game, 265)).Send();
        }
        private void Fire() => FireEmployee(GameData.Data.CompanyData.OfficeData);
        protected abstract void FireEmployee(OfficeData office);
        protected virtual bool CanFire() => true;
        public override void OnListUpdate(Employee param)
        {
            base.OnListUpdate(param);
            employeeItem.OnListUpdate(param);
        }
        #endregion methods
    }
}