using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    [System.Serializable]
    public class RentableOfficeShopCartBehaviour : RentablePremiseShopCartBehaviour<RentableOfficeShopItemData, RentableOffice>
    {
        #region fields & properties
        public override ShopData<RentableOfficeShopItemData> Data => GameData.Data.BrowserData.OfficeShop;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}