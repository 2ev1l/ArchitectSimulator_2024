using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RentableLandPlotCartData : SingleItemCartData<RentableLandPlotShopItemData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override bool CanPurchaseCart()
        {
            if (!base.CanPurchaseCart()) return false;
            if (!GameData.Data.CompanyData.LandPlotsData.CanAdd(base.CartItem.Item.Info.Id)) return false;
            return true;
        }
        #endregion methods
    }
}