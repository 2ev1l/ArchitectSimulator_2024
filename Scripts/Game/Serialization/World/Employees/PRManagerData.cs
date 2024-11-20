using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class PRManagerData : EmployeeData, ISingleEmployee, ICloneable<PRManagerData>
    {
        #region fields & properties
        protected override int MinSalary => 300;
        protected override int MaxSalary => 2500;
        protected override float SalaryIncreasePower => 1.8f;
        #endregion fields & properties

        #region methods
        public PRManagerData Clone()
        {
            PRManagerData clone = new(SkillLevel)
            {
                humanProfileId = this.humanProfileId,
                salary = this.salary,
            };
            return clone;
        }
        public override string ToLanguage() => LanguageInfo.GetTextByType(TextType.Game, 314);
        public PRManagerData(int skillLevel) : base(skillLevel) { }
        #endregion methods
    }
}