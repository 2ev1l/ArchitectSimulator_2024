using Game.Serialization.World;
using Game.UI.Overlay.Computer.BuildingConstructionApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class UnfinishedConstructionLandPlotItemList : UnfinishedConstructionItemList
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotBlueprintStarted += UpdateListData;
            plots.OnPlotBlueprintFinished += UpdateListData;
            plots.OnPlotAdded += UpdateListData;
            plots.OnPlotStartedSelling += UpdateListData;
            plots.OnPlotEndedSelling += UpdateListData;
            plots.OnPlotSold += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotBlueprintStarted -= UpdateListData;
            plots.OnPlotBlueprintFinished -= UpdateListData;
            plots.OnPlotAdded -= UpdateListData;
            plots.OnPlotStartedSelling -= UpdateListData;
            plots.OnPlotEndedSelling -= UpdateListData;
            plots.OnPlotSold -= UpdateListData;
        }
        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<ConstructionData> currentItemsReference)
        {
            currentItemsReference.Clear();
            LandPlotsData plotsData = GameData.Data.CompanyData.LandPlotsData;
            IReadOnlyList<LandPlotData> plots = plotsData.Plots;
            int plotsCount = plots.Count;

            for (int i = 0; i < plotsCount; ++i)
            {
                LandPlotData plot = plots[i];
                if (plotsData.IsSelling(plot.Id, out _)) continue;
                if (plotsData.IsSold(plot.Id, out _)) continue;
                ConstructionData cd = plot.ConstructionReference;
                if (cd == null) continue;
                if (cd.BuildCompletionMonth > -1) continue;
                currentItemsReference.Add(cd);
            }
        }
        #endregion methods
    }
}