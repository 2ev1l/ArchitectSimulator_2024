using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitHRData : RecruitEmployeeData<HRManagerData>, ICloneable<RecruitHRData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void Hire() => GameData.Data.CompanyData.OfficeData.TryHireHRManager(Employee);
        public RecruitHRData Clone() => new(Id, StartPrice, Discount, Employee);
        public RecruitHRData(int id, int startPrice, int discount, HRManagerData employee) : base(id, startPrice, discount, employee) { }
        #endregion methods
    }
}