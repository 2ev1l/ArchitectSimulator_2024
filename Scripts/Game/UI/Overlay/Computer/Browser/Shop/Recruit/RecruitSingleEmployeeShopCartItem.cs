using Game.Events;
using Game.Serialization.World;
using Game.UI.Elements;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitSingleEmployeeShopCartItem<Recruit, Employee> : RecruitEmployeeShopCartItem<Recruit, Employee>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData, ISingleEmployee
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override bool CanBuy()
        {
            RecruitSingleEmployeeCartData<Recruit, Employee> cart = (RecruitSingleEmployeeCartData<Recruit, Employee>)ShopCartData.Cart;
            if (cart.GetRole(GameData.Data.CompanyData.OfficeData.Divisions).IsEmployeeHired()) return false;
            return base.CanBuy();
        }
        #endregion methods
    }
}