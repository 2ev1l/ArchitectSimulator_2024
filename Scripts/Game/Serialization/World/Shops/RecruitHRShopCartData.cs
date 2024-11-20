using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitHRShopCartData : RecruitEmployeeShopCartData<RecruitHRData, HRManagerData>
    {
        #region fields & properties
        public override CartData<RecruitHRData> Cart => cart;
        [SerializeField] private RecruitHRCartData cart = new();
        #endregion fields & properties

        #region methods
        public override int GetMaxEmployees(int hrRating)
        {
            return Mathf.Max(base.GetMaxEmployees(hrRating) / 2, 1);
        }
        public override HRManagerData CreateNewEmployee(int rating) => new(rating);
        public override RecruitHRData CreateNewRecruit(int id, int startPrice, int discount, HRManagerData employee) => new(id, startPrice, discount, employee);
        #endregion methods
    }
}