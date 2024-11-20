using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitPRData : RecruitEmployeeData<PRManagerData>, ICloneable<RecruitPRData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void Hire() => GameData.Data.CompanyData.OfficeData.TryHirePRManager(Employee);
        public RecruitPRData Clone() => new(Id, StartPrice, Discount, Employee);
        public RecruitPRData(int id, int startPrice, int discount, PRManagerData employee) : base(id, startPrice, discount, employee) { }
        #endregion methods
    }
}