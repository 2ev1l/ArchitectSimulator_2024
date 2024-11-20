using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Universal.Core;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public abstract class RecruitEmployeeShopItem<Recruit, Employee> : VirtualShopItem<Recruit>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties
        [SerializeField] private EmployeeItem employeeItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(VirtualShopItemContext<Recruit> param)
        {
            base.OnListUpdate(param);
            employeeItem.OnListUpdate(param.ItemData.Item.Employee);
        }
        #endregion methods
    }
}