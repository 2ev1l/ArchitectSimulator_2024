using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableOfficeCartData : RentablePremiseCartData<RentableOfficeShopItemData, RentableOffice>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override ChangableInfoData<PremiseInfo> GetChangableInfo()
        {
            return GameData.Data.CompanyData.OfficeData;
        }
        #endregion methods
    }
}