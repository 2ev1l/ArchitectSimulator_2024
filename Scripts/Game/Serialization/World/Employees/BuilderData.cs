using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class BuilderData : EmployeeData, ICloneable<BuilderData>
    {
        #region fields & properties
        public UnityAction<bool> OnBusyStateChanged;
        protected override int MinSalary => 120;
        protected override int MaxSalary => 1200;
        protected override float SalaryIncreasePower => 1.8f;
        public bool IsBusy => isBusy;
        [SerializeField] private bool isBusy = false;
        #endregion fields & properties

        #region methods
        public void SetBusy()
        {
            isBusy = true;
            OnBusyStateChanged?.Invoke(true);
        }
        public void SetFree()
        {
            isBusy = false;
            OnBusyStateChanged?.Invoke(false);
        }
        protected override int RandomizeHumanProfileId()
        {
            int count = DB.Instance.HumanInfo.MaleList.Count;
            return DB.Instance.HumanInfo.MaleList[Random.Range(0, count)].Id;
        }
        public override string ToLanguage() => LanguageInfo.GetTextByType(TextType.Game, 172);
        public BuilderData Clone()
        {
            BuilderData clone = new(SkillLevel)
            {
                humanProfileId = this.humanProfileId,
                salary = this.salary,
            };
            return clone;
        }

        public BuilderData(int skillLevel) : base(skillLevel) { }
        #endregion methods
    }
}