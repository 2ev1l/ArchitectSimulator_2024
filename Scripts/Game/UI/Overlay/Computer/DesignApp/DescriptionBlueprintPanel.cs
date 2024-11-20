using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Universal.Core;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class DescriptionBlueprintPanel : DescriptionPanel<BlueprintData>
    {
        #region fields & properties
        [SerializeField] private DescriptionConstructionTaskPanel constructionTaskPanel;
        [SerializeField] private DescriptionLandPlotInfoPanel plotInfoPanel;
        [SerializeField] private DefaultStateMachine panelStates;
        [SerializeField] private PanelStateChange taskState;
        [SerializeField] private PanelStateChange landPlotState;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            BlueprintEditor.Instance.OnCurrentDataChanged += UpdateData;
            UpdateData();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            BlueprintEditor.Instance.OnCurrentDataChanged -= UpdateData;
        }
        private void UpdateData() => base.UpdateContextUI(BlueprintEditor.Instance.CurrentData);
        protected override void OnDataNull()
        {
            base.OnDataNull();
            if (ActiveItem != null)
            {
                ActiveItem.SetItemActive(false);
            }
            panelStates.ApplyDefaultState();
        }
        protected override void OnUpdateUI()
        {
            if (ActiveItem != null && ActiveItem.Context != Data)
            {
                ActiveItem.SetItemActive(false);
            }
            int refId = Data.BaseId;

            if (GameData.Data.CompanyData.ConstructionTasks.StartedTasks.Exists(x => x.BlueprintBaseIdReference == refId, out ConstructionTaskData task))
            {
                constructionTaskPanel.UpdateContextUI(task);
                panelStates.ApplyState(taskState);
                return;
            }
            if (GameData.Data.CompanyData.LandPlotsData.Plots.Exists(x => x.BlueprintBaseIdReference == refId, out LandPlotData plot))
            {
                plotInfoPanel.UpdateContextUI((LandPlotInfo)plot.Info);
                panelStates.ApplyState(landPlotState);
                return;
            }

            panelStates.ApplyDefaultState();
        }
        #endregion methods

    }
}