using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class HRManagerData : EmployeeData, ISingleEmployee, ICloneable<HRManagerData>
    {
        #region fields & properties
        protected override int MinSalary => 150;
        protected override int MaxSalary => 1500;
        protected override float SalaryIncreasePower => 1.8f;
        #endregion fields & properties

        #region methods
        public HRManagerData Clone()
        {
            HRManagerData clone = new(SkillLevel)
            {
                humanProfileId = this.humanProfileId,
                salary = this.salary,
            };
            return clone;
        }
        public override string ToLanguage() => LanguageInfo.GetTextByType(TextType.Game, 173);
        public HRManagerData(int skillLevel) : base(skillLevel) { }
        #endregion methods
    }
}