using Game.DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableOfficeShopItemData : RentablePremiseShopItemData<RentableOffice>, ICloneable<RentableOfficeShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<PremiseInfo> GetChangableInfo()
        {
            return GameData.Data.CompanyData.OfficeData;
        }
        protected override RentableOffice GetInfo()
        {
            int id = Id;
            return DB.Instance.RentableOfficeInfo.Find(x => x.Data.PremiseInfo.Id == id).Data;
        }
        public RentableOfficeShopItemData Clone() => new(Id, StartPrice, Discount);

        public RentableOfficeShopItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods

    }
}