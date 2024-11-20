using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableOfficeShopCartData : RentablePremiseShopCartData<RentableOfficeShopItemData, RentableOffice>
    {
        #region fields & properties
        public override CartData<RentableOfficeShopItemData> Cart => cart;
        [SerializeField] private RentableOfficeCartData cart = new();
        #endregion fields & properties

        #region methods
        protected override List<RentableOfficeShopItemData> GetNewData()
        {
            List<RentableOfficeShopItemData> result = new();
            foreach (var el in DB.Instance.RentableOfficeInfo.Data)
            {
                RentableOfficeShopItemData item = new(el.Data.PremiseInfo.Id, el.Data.Price, CustomMath.GetRandomChance(85) ? 0 : el.Data.GetRandomDiscount());
                result.Add(item);
            }
            result = result.OrderBy(x => x.FinalPrice).ToList();
            return result;
        }
        #endregion methods
    }
}