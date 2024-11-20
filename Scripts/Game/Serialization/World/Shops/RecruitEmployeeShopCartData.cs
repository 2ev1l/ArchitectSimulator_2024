using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RecruitEmployeeShopCartData<Recruit, Employee> : ShopCartData<Recruit>
        where Recruit : RecruitEmployeeData<Employee>, ICloneable<Recruit>
        where Employee : EmployeeData
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override List<Recruit> GetNewData()
        {
            List<Recruit> result = new();
            CompanyData company = GameData.Data.CompanyData;
            int companyRating = company.Rating.Value;
            int hrSkill = -1;
            IReadOnlyRole hrManager = company.OfficeData.Divisions.HRManager;
            if (hrManager.IsEmployeeHired())
            {
                hrSkill = hrManager.Employee.SkillLevel;
            }

            int choosedEmployeesCount = Mathf.Max(Random.Range(1, GetMaxEmployees(hrSkill)), 1);
            for (int i = 0; i < choosedEmployeesCount; ++i)
            {
                Employee employee = CreateNewEmployee(RandomizeRating(hrSkill, companyRating));
                Recruit rec = CreateNewRecruit(i, employee.Salary / 2, 0, employee);
                result.Add(rec);
            }
            return result;
        }
        public abstract Recruit CreateNewRecruit(int id, int startPrice, int discount, Employee employee);
        public abstract Employee CreateNewEmployee(int rating);
        public virtual int GetMaxEmployees(int hrSkill)
        {
            if (hrSkill < 0)
                return 1;
            int ratingPerEmployee = 10;
            return Mathf.Clamp((hrSkill + 30) / ratingPerEmployee, 3, 10);
        }
        protected int RandomizeRating(int hrSkill, int currentRating)
        {
            float randomScaleDown = 1.5f;
            float randomScaleUp = 1.2f;
            if (hrSkill > -1)
            {
                randomScaleUp += (hrSkill / 200f);

                randomScaleDown -= (hrSkill / 200f);
                randomScaleDown = Mathf.Max(randomScaleDown, 1);
            }
            return Mathf.Clamp(Mathf.RoundToInt(Random.Range(currentRating / randomScaleDown, currentRating * randomScaleUp)), 0, 100);
        }
        #endregion methods
    }
}