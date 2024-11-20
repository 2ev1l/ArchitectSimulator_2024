using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Universal.Core;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class DescriptionConstructionPanel : DescriptionPanel<ConstructionData>
    {
        #region fields & properties
        [SerializeField] private DescriptionConstructionTaskPanel constructionTaskPanel;
        [SerializeField] private DescriptionLandPlotInfoPanel plotInfoPanel;
        [SerializeField] private DefaultStateMachine panelStates;
        [SerializeField] private PanelStateChange taskState;
        [SerializeField] private PanelStateChange landPlotState;
        [SerializeField] private GameObject nextStepButton;
        [SerializeField] private SelectedConstructionPanel selectedConstructionPanel;
        [SerializeField] private List<UnfinishedConstructionItemList> unfinishedLists;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            foreach (var el in unfinishedLists)
                el.OnListUpdated += OnDataNull;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var el in unfinishedLists)
                el.OnListUpdated -= OnDataNull;
        }
        public void SendData()
        {
            selectedConstructionPanel.UpdateData(Data);
        }
        protected override void OnDataNull()
        {
            base.OnDataNull();
            if (ActiveItem != null)
            {
                ActiveItem.SetItemActive(false);
            }
            panelStates.ApplyDefaultState();
            nextStepButton.SetActive(false);
        }

        protected override void OnUpdateUI()
        {
            if (ActiveItem != null && (ActiveItem.Context != Data || !ActiveItem.isActiveAndEnabled))
            {
                OnDataNull();
                return;
            }
            int baseId = Data.BaseId;
            if (GameData.Data.CompanyData.ConstructionTasks.StartedTasks.Exists(x => x.BlueprintBaseIdReference == baseId, out ConstructionTaskData task))
            {
                constructionTaskPanel.UpdateContextUI(task);
                panelStates.ApplyState(taskState);
                nextStepButton.SetActive(true);
                return;
            }
            if (GameData.Data.CompanyData.LandPlotsData.Plots.Exists(x => x.BlueprintBaseIdReference == baseId, out LandPlotData plot))
            {
                plotInfoPanel.UpdateContextUI((LandPlotInfo)plot.Info);
                panelStates.ApplyState(landPlotState);
                nextStepButton.SetActive(true);
                return;
            }

            nextStepButton.SetActive(false);
            panelStates.ApplyDefaultState();
        }
        #endregion methods

    }
}