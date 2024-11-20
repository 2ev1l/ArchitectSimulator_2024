using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class RecruitBuilderShopCartItemList : RecruitEmployeeDivisionShopCartItemList<RecruitBuilderData, BuilderData>
    {
        #region fields & properties
        protected override IReadOnlyDivision Division => GameData.Data.CompanyData.OfficeData.Divisions.Builders;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}