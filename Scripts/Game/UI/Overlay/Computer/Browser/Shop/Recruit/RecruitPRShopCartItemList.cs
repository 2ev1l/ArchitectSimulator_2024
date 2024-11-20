using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RecruitPRShopCartItemList : RecruitEmployeeRoleShopCartItemList<RecruitPRData, PRManagerData>
    {
        #region fields & properties
        protected override IReadOnlyRole Role => GameData.Data.CompanyData.OfficeData.Divisions.PRManager;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}