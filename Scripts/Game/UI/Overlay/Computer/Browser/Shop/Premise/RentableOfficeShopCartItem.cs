using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RentableOfficeShopCartItem : RentablePremiseShopCartItem<RentableOfficeShopItemData, RentableOffice>
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