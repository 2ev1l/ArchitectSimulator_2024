using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class DesignEngineerData : EmployeeData, ISingleEmployee, ICloneable<DesignEngineerData>
    {
        #region fields & properties
        protected override int MinSalary => 250;
        protected override int MaxSalary => 3000;
        protected override float SalaryIncreasePower => 2f;
        #endregion fields & properties

        #region methods
        public DesignEngineerData Clone()
        {
            DesignEngineerData clone = new(SkillLevel)
            {
                humanProfileId = this.humanProfileId,
                salary = this.salary,
            };
            return clone;
        }
        public override string ToLanguage() => LanguageInfo.GetTextByType(TextType.Game, 316);
        public DesignEngineerData(int skillLevel) : base(skillLevel) { }
        #endregion methods
    }
}