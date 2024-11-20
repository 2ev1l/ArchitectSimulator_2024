using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitDesignEngineerShopCartData : RecruitEmployeeShopCartData<RecruitDesignEngineerData, DesignEngineerData>
    {
        #region fields & properties
        public override CartData<RecruitDesignEngineerData> Cart => cart;
        [SerializeField] private RecruitDesignEngineerCartData cart = new();
        #endregion fields & properties

        #region methods
        public override int GetMaxEmployees(int hrRating)
        {
            return Mathf.Max(base.GetMaxEmployees(hrRating) / 2, 1);
        }
        public override DesignEngineerData CreateNewEmployee(int rating) => new(rating);
        public override RecruitDesignEngineerData CreateNewRecruit(int id, int startPrice, int discount, DesignEngineerData employee) => new(id, startPrice, discount, employee);
        #endregion methods
    }
}