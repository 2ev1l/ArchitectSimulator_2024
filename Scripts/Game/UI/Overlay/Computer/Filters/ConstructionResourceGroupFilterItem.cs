using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Filters;

namespace Game.UI.Overlay.Computer
{
    [System.Serializable]
    public class ConstructionResourceGroupFilterItem : TextFilterItem<ConstructionResourceGroupType>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            Text.text = Value.ToLanguage();
        }
        #endregion methods
    }
}