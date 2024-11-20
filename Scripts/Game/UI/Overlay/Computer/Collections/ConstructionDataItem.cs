using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class ConstructionDataItem : ContextActionsItem<ConstructionData>
    {
        #region fields & properties
        [SerializeField] private BuildingDataItem buildingDataItem;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI resourcesCountText;
        [SerializeField] private TextMeshProUGUI roomsCountText;
        [SerializeField] private TextMeshProUGUI currentBuildersText;
        [SerializeField] private TextMeshProUGUI timeLeftText;
        [SerializeField] private TextMeshProUGUI timeToCompleteText;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            Context.OnBuildersChanged += UpdateUI;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            Context.OnBuildersChanged -= UpdateUI;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            if (nameText != null)
                nameText.text = $"{Context.Name}";
            if (resourcesCountText != null)
                resourcesCountText.text = $"{Context.BlueprintResources.Count}x";
            if (roomsCountText != null)
                roomsCountText.text = $"{Context.BlueprintRooms.Count}x";
            if (currentBuildersText != null)
                currentBuildersText.text = $"{Context.Builders.Count}";
            if (timeLeftText != null)
            {
                if (Context.BuildCompletionMonth >= 0)
                {
                    int timeLeft = Context.BuildCompletionMonth - GameData.Data.PlayerData.MonthData.CurrentMonth;
                    timeLeft = Mathf.Max(timeLeft, 0);
                    timeLeftText.text = $"{timeLeft} m.";
                }
                else
                    timeLeftText.text = $"???";
            }
            if (timeToCompleteText != null)
            {
                int cmplTime = Context.GetCompletionTime(Context.Builders);
                if (cmplTime >= 0)
                    timeLeftText.text = $"{cmplTime} m.";
                else
                    timeLeftText.text = $"???";
            }
        }
        public override void OnListUpdate(ConstructionData param)
        {
            base.OnListUpdate(param);
            buildingDataItem.OnListUpdate(param.BuildingData);
        }
        #endregion methods
    }
}