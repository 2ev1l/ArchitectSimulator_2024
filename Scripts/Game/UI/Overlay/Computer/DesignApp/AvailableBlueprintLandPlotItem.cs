using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class AvailableBlueprintLandPlotItem : AvailableBlueprintItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            int id = Context.Id;
            LandPlotsData plotsData = GameData.Data.CompanyData.LandPlotsData;
            if (plotsData.Plots.Exists(x => x.PlotInfo.BlueprintInfo.Id == id, out LandPlotData landPlot))
            {
                ChangeName($"B-{Context.Id:000} | {landPlot.Info.NameInfo.Text}");
            }
        }
        protected override void OnBlueprintAdded(BlueprintData added)
        {
            base.OnBlueprintAdded(added);
            int blueprintId = Context.Id;
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            if (plots.Plots.Exists(x => ((LandPlotInfo)x.Info).BlueprintInfo.Id == blueprintId, out LandPlotData exist))
            {
                plots.TryStartBlueprint(exist.Id, added.BaseId);
            }
        }
        #endregion methods
    }
}