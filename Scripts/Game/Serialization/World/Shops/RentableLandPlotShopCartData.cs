using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableLandPlotShopCartData : ShopCartData<RentableLandPlotShopItemData>
    {
        #region fields & properties
        public override CartData<RentableLandPlotShopItemData> Cart => cart;
        [SerializeField] private RentableLandPlotCartData cart = new();
        #endregion fields & properties

        #region methods
        protected override List<RentableLandPlotShopItemData> GetNewData()
        {
            List<RentableLandPlotShopItemData> result = new();
            LandPlotsData plots = GameData.Data.CompanyData.LandPlotsData;
            foreach (var el in DB.Instance.RentableLandPlotInfo.Data)
            {
                if (plots.Exists(el.Data.PremiseInfo.Id, out _)) continue;
                RentableLandPlotShopItemData item = new(el.Data.PremiseInfo.Id, el.Data.Price, CustomMath.GetRandomChance(90) ? 0 : el.Data.GetRandomDiscount());
                result.Add(item);
            }
            result = result.OrderBy(x => x.FinalPrice).ToList();
            return result;
        }
        #endregion methods
    }
}