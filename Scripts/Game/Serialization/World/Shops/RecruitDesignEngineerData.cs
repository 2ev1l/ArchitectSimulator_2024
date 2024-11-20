using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RecruitDesignEngineerData : RecruitEmployeeData<DesignEngineerData>, ICloneable<RecruitDesignEngineerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void Hire() => GameData.Data.CompanyData.OfficeData.TryHireDesignEngineer(Employee);
        public RecruitDesignEngineerData Clone() => new(Id, StartPrice, Discount, Employee);
        public RecruitDesignEngineerData(int id, int startPrice, int discount, DesignEngineerData employee) : base(id, startPrice, discount, employee) { }
        #endregion methods
    }
}