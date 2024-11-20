using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RecruitHRShopCartBehaviour : RecruitEmployeeShopCartBehaviour<RecruitHRData, HRManagerData>
    {
        #region fields & properties
        public override ShopData<RecruitHRData> Data => GameData.Data.BrowserData.HRRecruit;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}