using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class SellingLandPlotData
    {
        #region fields & properties
        public LandPlotData Plot => plot;
        [SerializeField] private LandPlotData plot;
        public int SellingPrice => sellingPrice;
        [SerializeField] private int sellingPrice = 0;
        #endregion fields & properties

        #region methods
        public SellingLandPlotData(int sellingPrice, LandPlotData landPlotData)
        {
            this.sellingPrice = Mathf.Max(sellingPrice, 1);
            this.plot = landPlotData;
        }
        #endregion methods
    }
}