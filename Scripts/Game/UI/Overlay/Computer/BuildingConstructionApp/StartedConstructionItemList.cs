using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public abstract class StartedConstructionItemList : InfinityFilteredItemListBase<StartedConstructionItem, ConstructionData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            ConstructionsData constructions = GameData.Data.ConstructionsData;
            constructions.OnConstructionAdded += UpdateListData;
            constructions.OnConstructionBuilded += UpdateListData;
            constructions.OnConstructionBuildCanceled += UpdateListData;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ConstructionsData constructions = GameData.Data.ConstructionsData;
            constructions.OnConstructionAdded -= UpdateListData;
            constructions.OnConstructionBuilded -= UpdateListData;
            constructions.OnConstructionBuildCanceled -= UpdateListData;
        }
        private void UpdateListData(ConstructionData _) => UpdateListData();
        #endregion methods
    }
}