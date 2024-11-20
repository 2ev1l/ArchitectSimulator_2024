using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class StartedLandPlotConstructionItemList : StartedConstructionItemList
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
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotBlueprintStarted -= UpdateListData;
            plots.OnPlotBlueprintFinished -= UpdateListData;
            plots.OnPlotAdded -= UpdateListData;
        }
        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<ConstructionData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<LandPlotData> plots = GameData.Data.CompanyData.LandPlotsData.Plots;
            int count = plots.Count;

            for (int i = 0; i < count; ++i)
            {
                LandPlotData plot = plots[i];
                ConstructionData construction = plot.ConstructionReference;
                if (construction == null) continue;
                if (construction.IsBuilded) continue;
                if (construction.BuildCompletionMonth < 0) continue;
                currentItemsReference.Add(construction);
            }
        }
        #endregion methods
    }
}