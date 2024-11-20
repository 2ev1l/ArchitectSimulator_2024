using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class VehicleShopCartData : ShopCartData<VehicleShopItemData>
    {
        #region fields & properties
        public override CartData<VehicleShopItemData> Cart => cart;
        [SerializeField] private VehicleCartData cart = new();
        #endregion fields & properties

        #region methods
        protected override List<VehicleShopItemData> GetNewData()
        {
            List<VehicleShopItemData> result = new();
            foreach (var el in DB.Instance.BuyableVehicleInfo.Data)
            {
                VehicleShopItemData item = new(el.Data.Info.Id, el.Data.Price, CustomMath.GetRandomChance(85) ? 0 : el.Data.GetRandomDiscount());
                result.Add(item);
            }
            result = result.OrderBy(x => x.FinalPrice).ToList();
            return result;
        }
        #endregion methods
    }
}