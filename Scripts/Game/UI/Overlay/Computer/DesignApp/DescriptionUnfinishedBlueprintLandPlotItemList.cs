using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class DescriptionUnfinishedBlueprintLandPlotItemList : DescriptionUnfinishedBlueprintItemList
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

        protected override void UpdateCurrentItems(List<BlueprintData> currentItemsReference)
        {
            currentItemsReference.Clear();
            LandPlotsData plotsData = GameData.Data.CompanyData.LandPlotsData;
            IReadOnlyList<LandPlotData> plots = plotsData.Plots;
            int plotsCount = plots.Count;
            BlueprintsData blueprints = GameData.Data.BlueprintsData;

            for (int i = 0; i < plotsCount; ++i)
            {
                LandPlotData plot = plots[i];
                if (plotsData.IsSelling(plot.Id, out _)) continue;
                if (plotsData.IsSold(plot.Id, out _)) continue;
                if (!blueprints.TryGetByBaseId(plot.BlueprintBaseIdReference, out BlueprintData bp)) continue;
                currentItemsReference.Add(bp);
            }
        }
        #endregion methods
    }
}