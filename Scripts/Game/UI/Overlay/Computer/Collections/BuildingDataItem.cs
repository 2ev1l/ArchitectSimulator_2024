using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class BuildingDataItem : ContextActionsItem<BuildingInfo>
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI buildingTypeText;
        [SerializeField] private TextMeshProUGUI buildingStyleText;
        [SerializeField] private TextMeshProUGUI maxFloorText;
        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (buildingTypeText != null)
                buildingTypeText.text = Context.BuildingType.ToLanguage();
            if (buildingStyleText != null)
                buildingStyleText.text = Context.BuildingStyle.ToLanguage();
            if (maxFloorText != null)
                maxFloorText.text = $"{BuildingFloor.F1_Flooring.ToLanguage()} .. {Context.MaxFloor.ToLanguage()}";
        }
        #endregion methods
    }
}