using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class AvailableBlueprintLandPlotItemList : AvailableBlueprintItemList<AvailableBlueprintLandPlotItem>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotAdded += UpdateListData;
            plots.OnPlotBlueprintStarted += UpdateListData;
            plots.OnPlotBlueprintFinished += UpdateListData;
            plots.OnPlotStartedSelling += UpdateListData;
            plots.OnPlotEndedSelling += UpdateListData;
            plots.OnPlotSold += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotAdded -= UpdateListData;
            plots.OnPlotBlueprintStarted -= UpdateListData;
            plots.OnPlotBlueprintFinished -= UpdateListData;
            plots.OnPlotStartedSelling -= UpdateListData;
            plots.OnPlotEndedSelling -= UpdateListData;
            plots.OnPlotSold -= UpdateListData;
        }
        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<BlueprintInfo> currentItemsReference)
        {
            currentItemsReference.Clear();
            LandPlotsData plotsData = GameData.Data.CompanyData.LandPlotsData;
            IReadOnlyList<LandPlotData> plots = plotsData.Plots;
            int count = plots.Count;
            for (int i = 0; i < count; ++i)
            {
                LandPlotData plot = plots[i];
                if (plot.BlueprintBaseIdReference > -1) continue;
                if (plotsData.IsSelling(plot.Id, out _)) continue;
                if (plotsData.IsSold(plot.Id, out _)) continue;
                BlueprintInfo blueprint = ((LandPlotInfo)plot.Info).BlueprintInfo;
                currentItemsReference.Add(blueprint);
            }
        }
        #endregion methods
    }
}