using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableLandPlotShopItemData : RentableObjectItemData<RentableLandPlot>, ICloneable<RentableLandPlotShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            base.OnPurchase(count);
            GameData.Data.CompanyData.LandPlotsData.TryAdd(Id);
        }

        protected override RentableLandPlot GetInfo()
        {
            int id = Id;
            return DB.Instance.RentableLandPlotInfo.Find(x => x.Data.PremiseInfo.Id == id).Data;
        }

        public RentableLandPlotShopItemData Clone() => new(Id, StartPrice, Discount);
        public RentableLandPlotShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods

    }
}