using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class AnyChecker : ResultChecker
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Always true</returns>
        public override bool GetResult()
        {
            return true;
        }
        #endregion methods
    }
}