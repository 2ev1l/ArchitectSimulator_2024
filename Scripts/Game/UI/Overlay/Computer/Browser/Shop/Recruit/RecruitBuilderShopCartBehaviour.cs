using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RecruitBuilderShopCartBehaviour : RecruitEmployeeShopCartBehaviour<RecruitBuilderData, BuilderData>
    {
        #region fields & properties
        public override ShopData<RecruitBuilderData> Data => GameData.Data.BrowserData.BuildersRecruit;

        #endregion fields & properties

        #region methods

        #endregion methods
    }
}