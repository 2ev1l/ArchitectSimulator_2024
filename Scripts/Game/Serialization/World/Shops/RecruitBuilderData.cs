using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitBuilderData : RecruitEmployeeData<BuilderData>, ICloneable<RecruitBuilderData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void Hire() => GameData.Data.CompanyData.OfficeData.TryHireBuilder(base.Employee);
        public RecruitBuilderData Clone() => new(Id, StartPrice, Discount, Employee);
        public RecruitBuilderData(int id, int startPrice, int discount, BuilderData employee) : base(id, startPrice, discount, employee) { }
        #endregion methods

    }
}