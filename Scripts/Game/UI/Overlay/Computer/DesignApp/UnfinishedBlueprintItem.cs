using Game.Serialization.World;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class UnfinishedBlueprintItem : BlueprintDataItem
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override bool IsNewNameAllowed(string name)
        {
            return base.IsNewNameAllowed(name) && !GameData.Data.BlueprintsData.ContainName(name);
        }
        #endregion methods
    }
}