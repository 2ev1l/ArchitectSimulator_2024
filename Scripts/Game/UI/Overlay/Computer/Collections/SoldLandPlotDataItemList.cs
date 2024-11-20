using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Collections
{
    public class SoldLandPlotDataItemList : InfinityFilteredItemListBase<SellingLandPlotDataItem, SellingLandPlotData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotSold += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotSold -= UpdateListData;
        }
        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<SellingLandPlotData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<SellingLandPlotData> plots = GameData.Data.CompanyData.LandPlotsData.SoldPlots;
            int count = plots.Count;
            for (int i = 0; i < count; ++i)
            {
                currentItemsReference.Add(plots[i]);
            }
        }
        #endregion methods
    }
}