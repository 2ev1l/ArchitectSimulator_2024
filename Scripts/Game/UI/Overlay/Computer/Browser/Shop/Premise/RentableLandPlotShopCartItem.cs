using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RentableLandPlotShopCartItem : RentableObjectShopCartItem<RentableLandPlotShopItemData, RentableLandPlot, PremiseInfo>
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override bool CanBuy()
        {
            if (!base.CanBuy()) return false;
            if (!GameData.Data.CompanyData.LandPlotsData.CanAdd(DBInfo.Id)) return false;
            return true;
        }
        protected override bool IsSoldOut()
        {
            if (base.IsSoldOut()) return true;
            if (GameData.Data.CompanyData.LandPlotsData.Exists(DBInfo.Id, out _)) return true;
            return false;
        }
        protected override PremiseInfo GetDBInfo(RentableLandPlot rentableContext)
        {
            return rentableContext.PremiseInfo;
        }
        #endregion methods
    }
}