using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class HiredBuilderItem : HiredEmployeeItem<BuilderData>
    {
        #region fields & properties
        [SerializeField] private GameObject raycastBlock;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            Context.OnBusyStateChanged += UpdateUI;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            Context.OnBusyStateChanged -= UpdateUI;
        }
        private void UpdateUI(bool _) => UpdateUI();
        protected override void UpdateUI()
        {
            base.UpdateUI();
            bool blockActive = !CanFire();
            if (raycastBlock.activeSelf != blockActive)
                raycastBlock.SetActive(blockActive);
        }
        protected override bool CanFire()
        {
            if (Context.IsBusy) return false;
            return base.CanFire();
        }
        protected override void FireEmployee(OfficeData office)
        {
            office.FireBuilder(Context);
        }
        #endregion methods
    }
}