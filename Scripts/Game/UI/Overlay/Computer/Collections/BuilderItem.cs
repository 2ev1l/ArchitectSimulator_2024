using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class BuilderItem : EmployeeItem
    {
        #region fields & properties
        [SerializeField] private GameObject busyIndicator;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (busyIndicator != null)
            {
                bool isBusy = ((BuilderData)Context).IsBusy;
                if (busyIndicator.activeSelf != isBusy)
                    busyIndicator.SetActive(isBusy);
            }

        }
        #endregion methods
    }
}