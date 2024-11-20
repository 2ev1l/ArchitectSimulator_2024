using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.Collections
{
    public class LandPlotOfferDataItemList : InfinityFilteredItemListBase<LandPlotOfferDataItem, LandPlotOfferData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotSold += UpdateListData;
            plots.OnPlotEndedSelling += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            plots.OnPlotSold -= UpdateListData;
            plots.OnPlotEndedSelling -= UpdateListData;
        }
        private void UpdateListData(LandPlotData _) => UpdateListData();
        protected override void UpdateCurrentItems(List<LandPlotOfferData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<LandPlotOfferData> offers = GameData.Data.CompanyData.LandPlotsData.Offers;
            int offersCount = offers.Count;
            for (int i = 0; i < offersCount; ++i)
            {
                currentItemsReference.Add(offers[i]);
            }
        }
        #endregion methods
    }
}