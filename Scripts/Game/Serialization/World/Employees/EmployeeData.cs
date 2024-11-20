using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.DataBase;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class EmployeeData : IEmployee
    {
        #region fields & properties
        /// <summary>
        /// Salary at 0 skill
        /// </summary>
        protected abstract int MinSalary { get; }
        /// <summary>
        /// Salary at 100 skill
        /// </summary>
        protected abstract int MaxSalary { get; }
        /// <summary>
        /// Higher value (1+) means less increase in start and less increase in end. <br></br>
        /// Lower value (1-) means big increase in start and less increase in end. <br></br>
        /// </summary>
        protected virtual float SalaryIncreasePower => 1.2f;

        public int Id => id;
        [SerializeField][Min(0)] private int id = 0;
        public int SkillLevel => skillLevel;
        [SerializeField][Range(0, 100)] private int skillLevel;
        public int Salary => salary;
        [SerializeField][Min(1)] protected int salary = 1;
        public HumanInfo HumanInfo
        {
            get
            {
                humanInfo ??= DB.Instance.HumanInfo[HumanProfileId].Data;
                return humanInfo;
            }
        }
        [System.NonSerialized] HumanInfo humanInfo = null;
        private int HumanProfileId
        {
            get
            {
                if (humanProfileId == -1)
                {
                    humanProfileId = RandomizeHumanProfileId();
                }
                return humanProfileId;
            }
        }
        [SerializeField][Min(-1)] protected int humanProfileId = -1;
        #endregion fields & properties

        #region methods
        internal void ChangeId(int id) => this.id = Mathf.Max(id, 0);
        private int CalculateSalary()
        {
            float pow = SalaryIncreasePower;
            float pow100 = Mathf.Pow(100, pow);
            float powSkill = Mathf.Pow(skillLevel, pow);
            float skillPercent = powSkill / pow100;
            return Mathf.RoundToInt(Mathf.Lerp(MinSalary, MaxSalary, skillPercent));
        }
        protected virtual int RandomizeHumanProfileId()
        {
            return Random.Range(0, DB.Instance.HumanInfo.Data.Count - 1);
        }
        protected virtual int RandomizeSalary(int calculatedSalary)
        {
            return Mathf.RoundToInt(Random.Range(calculatedSalary / 1.2f, calculatedSalary * 1.2f));
        }
        public abstract string ToLanguage();
        public EmployeeData(int skillLevel)
        {
            skillLevel = Mathf.Clamp(skillLevel, 0, 100);
            this.skillLevel = skillLevel;
            int salary = CalculateSalary();
            salary = RandomizeSalary(salary);
            this.salary = salary;
        }
        #endregion methods
    }
}