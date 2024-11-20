using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class DesignEngineerRoleData : RoleData<DesignEngineerData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        private int CalculateFreeTimeForEmployee(DesignEngineerData employee)
        {
            int result = 10;
            float freeTimePerSkill = 0.7f;
            result += (int)(employee.SkillLevel * freeTimePerSkill);
            return result;
        }
        protected override void OnEmployeeHired(DesignEngineerData newEmployee)
        {
            base.OnEmployeeHired(newEmployee);
            RangedValue freeTime = GameData.Data.PlayerData.MonthData.FreeTime;
            int timeAdd = CalculateFreeTimeForEmployee(newEmployee);
            freeTime.SetMaxRange(MonthData.BaseFreeTime + timeAdd);
            freeTime.TryIncreaseValue(timeAdd);
        }
        protected override void OnEmployeeFired(DesignEngineerData oldEmployee)
        {
            base.OnEmployeeFired(oldEmployee);
            RangedValue freeTime = GameData.Data.PlayerData.MonthData.FreeTime;
            int timeAdd = CalculateFreeTimeForEmployee(oldEmployee);
            freeTime.TryDecreaseValue(timeAdd);
            freeTime.SetMaxRange(MonthData.BaseFreeTime);
        }
        #endregion methods
    }
}