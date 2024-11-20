using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RecruitPRShopCartBehaviour : RecruitEmployeeShopCartBehaviour<RecruitPRData, PRManagerData>
    {
        #region fields & properties
        public override ShopData<RecruitPRData> Data => GameData.Data.BrowserData.PRRecruit;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}