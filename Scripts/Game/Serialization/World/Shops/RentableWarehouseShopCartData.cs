using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableWarehouseShopCartData : RentablePremiseShopCartData<RentableWarehouseShopItemData, RentableWarehouse>
    {
        #region fields & properties
        public override CartData<RentableWarehouseShopItemData> Cart => cart;
        [SerializeField] private RentableWarehouseCartData cart = new();
        #endregion fields & properties

        #region methods
        protected override List<RentableWarehouseShopItemData> GetNewData()
        {
            List<RentableWarehouseShopItemData> result = new();
            foreach (var el in DB.Instance.RentableWarehouseInfo.Data)
            {
                if (!el.Data.VisibleInShop) continue;
                RentableWarehouseShopItemData item = new(el.Data.PremiseInfo.Id, el.Data.Price, CustomMath.GetRandomChance(85) ? 0 : el.Data.GetRandomDiscount());
                result.Add(item);
            }
            result = result.OrderBy(x => x.FinalPrice).ToList();
            return result;
        }
        #endregion methods
    }
}