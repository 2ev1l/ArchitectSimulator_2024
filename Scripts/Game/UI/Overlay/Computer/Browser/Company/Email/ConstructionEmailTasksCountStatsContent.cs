using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class ConstructionEmailTasksCountStatsContent : TextStatsContent
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override void UpdateUI()
        {
            Text.text = $"{GameData.Data.CompanyData.ConstructionTasks.StartedTasks.Count}x";
        }
        #endregion methods
    }
}