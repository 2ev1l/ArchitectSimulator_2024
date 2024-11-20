using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableWarehouseShopItemData : RentablePremiseShopItemData<RentableWarehouse>, ICloneable<RentableWarehouseShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<PremiseInfo> GetChangableInfo()
        {
            return GameData.Data.CompanyData.WarehouseData;
        }
        protected override RentableWarehouse GetInfo()
        {
            int id = Id;
            return DB.Instance.RentableWarehouseInfo.Find(x => x.Data.PremiseInfo.Id == id).Data;
        }
        public RentableWarehouseShopItemData Clone() => new(Id, StartPrice, Discount);
        public RentableWarehouseShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}