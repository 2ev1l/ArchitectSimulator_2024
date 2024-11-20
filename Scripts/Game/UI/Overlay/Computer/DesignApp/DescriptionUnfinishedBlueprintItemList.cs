using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public abstract class DescriptionUnfinishedBlueprintItemList : DescriptionItemList<BlueprintData>
    {
        #region fields & properties
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.BlueprintsData.OnBlueprintAdded += UpdateListData;
            GameData.Data.BlueprintsData.OnBlueprintRemoved += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.BlueprintsData.OnBlueprintAdded -= UpdateListData;
            GameData.Data.BlueprintsData.OnBlueprintRemoved -= UpdateListData;
        }
        private void UpdateListData(BlueprintData _) => UpdateListData();
        #endregion methods
    }
}