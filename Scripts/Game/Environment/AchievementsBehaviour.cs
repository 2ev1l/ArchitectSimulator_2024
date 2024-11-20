using Game.Environment.Observers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment
{
    public class AchievementsBehaviour : MonoBehaviour
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        public void SetAchievement(string name)
        {
            AchievementsObserver.SetAchievement(name);
        }
        #endregion methods
    }
}