using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitPRShopCartData : RecruitEmployeeShopCartData<RecruitPRData, PRManagerData>
    {
        #region fields & properties
        public override CartData<RecruitPRData> Cart => cart;
        [SerializeField] private RecruitPRCartData cart = new();
        #endregion fields & properties

        #region methods
        public override int GetMaxEmployees(int hrRating)
        {
            return Mathf.Max(base.GetMaxEmployees(hrRating) / 2, 1);
        }
        public override PRManagerData CreateNewEmployee(int rating) => new(rating);
        public override RecruitPRData CreateNewRecruit(int id, int startPrice, int discount, PRManagerData employee) => new(id, startPrice, discount, employee);
        #endregion methods
    }
}