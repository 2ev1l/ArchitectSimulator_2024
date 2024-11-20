using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitBuilderShopCartData : RecruitEmployeeShopCartData<RecruitBuilderData, BuilderData>
    {
        #region fields & properties
        public override CartData<RecruitBuilderData> Cart => cart;
        [SerializeField] private RecruitBuilderCartData cart = new();
        #endregion fields & properties

        #region methods
        public override BuilderData CreateNewEmployee(int rating) => new(rating);
        public override RecruitBuilderData CreateNewRecruit(int id, int startPrice, int discount, BuilderData employee) => new(id, startPrice, discount, employee);
        #endregion methods
    }
}