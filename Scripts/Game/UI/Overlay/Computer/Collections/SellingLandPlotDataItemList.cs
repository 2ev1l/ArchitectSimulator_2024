using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Collections
{
    public class SellingLandPlotDataItemList : InfinityFilteredItemListBase<SellingLandPlotDataItem, SellingLandPlotData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotSold += UpdateListData;
            plots.OnPlotStartedSelling += UpdateListData;
            plots.OnPlotEndedSelling += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotSold -= UpdateListData;
            plots.OnPlotStartedSelling -= UpdateListData;
            plots.OnPlotEndedSelling -= UpdateListData;
        }

        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<SellingLandPlotData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<SellingLandPlotData> plots = GameData.Data.CompanyData.LandPlotsData.SellingPlots;
            int count = plots.Count;
            for (int i = 0; i < count; ++i)
            {
                currentItemsReference.Add(plots[i]);
            }
        }
        #endregion methods
    }
}