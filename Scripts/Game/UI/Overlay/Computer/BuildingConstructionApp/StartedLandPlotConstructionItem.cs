using Game.Events;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class StartedLandPlotConstructionItem : StartedConstructionItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnBeforeBuildCompleted()
        {
            base.OnBeforeBuildCompleted();
            int reference = Context.BaseId;
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.Plots.Exists(x => x.BlueprintBaseIdReference == reference, out LandPlotData plot);
            if (plot == null)
            {
                InfoRequest.GetErrorRequest(402).Send();
                return;
            }
            plots.TryFinishBlueprint(plot.Id);
        }
        #endregion methods
    }
}