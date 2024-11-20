using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class ConstructionResourceShopCartData : ResourceShopCartData<ConstructionResourceShopItemData, BuyableConstructionResource>
    {
        #region fields & properties
        public override CartData<ConstructionResourceShopItemData> Cart => cart;
        [SerializeField] private ConstructionResourceCartData cart = new();
        #endregion fields & properties

        #region methods

        protected override List<ConstructionResourceShopItemData> GetNewData()
        {
            List<ConstructionResourceShopItemData> result = new();
            int discountChance = 12;
            foreach (var el in DB.Instance.BuyableConstructionResourceInfo.Data)
            {
                ConstructionResourceShopItemData item = new(el.Data.ResourceInfo.Id, el.Data.Price, CustomMath.GetRandomChance(discountChance) ? el.Data.GetRandomDiscount() : 0);
                result.Add(item);
            }
            return result;
        }
        #endregion methods
    }
}