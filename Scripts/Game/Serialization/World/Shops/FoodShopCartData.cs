using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class FoodShopCartData : ShopCartData<FoodShopItemData>
    {
        #region fields & properties
        public override CartData<FoodShopItemData> Cart => cart;
        [SerializeField] private FoodCartData cart = new();
        #endregion fields & properties

        #region methods
        protected override List<FoodShopItemData> GetNewData()
        {
            List<FoodShopItemData> result = new();
            foreach (var el in DB.Instance.BuyableFoodInfo.Data)
            {
                result.Add(new(el.Data.Info.Id, el.Data.Price, CustomMath.GetRandomChance(80) ? 0 : el.Data.MaxDiscount));
            }
            result = result.OrderBy(x => x.FinalPrice).ToList();
            return result;
        }
        #endregion methods
    }
}