using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Browser.LandPlots
{
    public class OwnLandPlotItemList : InfinityFilteredItemListBase<OwnLandPlotItem, LandPlotData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotAdded += UpdateListData;
            plots.OnPlotBlueprintFinished += UpdateListData;
            plots.OnPlotEndedSelling += UpdateListData;
            plots.OnPlotStartedSelling += UpdateListData;
            plots.OnPlotSold += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotAdded -= UpdateListData;
            plots.OnPlotBlueprintFinished -= UpdateListData;
            plots.OnPlotEndedSelling -= UpdateListData;
            plots.OnPlotStartedSelling -= UpdateListData;
            plots.OnPlotSold -= UpdateListData;
        }
        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<LandPlotData> currentItemsReference)
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
                currentItemsReference.Add(plot);
            }
        }
        #endregion methods
    }
}