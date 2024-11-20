using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RecruitDesignEngineerShopCartBehaviour : RecruitEmployeeShopCartBehaviour<RecruitDesignEngineerData, DesignEngineerData>
    {
        #region fields & properties
        public override ShopData<RecruitDesignEngineerData> Data => GameData.Data.BrowserData.DesignEngineerRecruit;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}