using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class HouseShopCartData : RealEstateShopCartData<HouseShopItemData, RentableHouse>
    {
        #region fields & properties
        public override CartData<HouseShopItemData> Cart => cart;
        [SerializeField] private HouseCartData cart = new();
        #endregion fields & properties

        #region methods
        protected override List<HouseShopItemData> GetNewData()
        {
            List<HouseShopItemData> result = new();
            foreach (var el in DB.Instance.RentableHouseInfo.Data)
            {
                HouseShopItemData item = new(el.Data.RealEstateInfo.Id, el.Data.Price, CustomMath.GetRandomChance(95) ? 0 : el.Data.GetRandomDiscount());
                result.Add(item);
            }
            result = result.OrderBy(x => x.FinalPrice).ToList();
            return result;
        }
        #endregion methods
    }
}