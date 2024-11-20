using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public abstract class UnfinishedConstructionItemList : DescriptionItemList<ConstructionData>
    {
        #region fields & properties
        public UnityAction OnListUpdated;
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
        public override void UpdateListData()
        {
            base.UpdateListData();
            OnListUpdated?.Invoke();
        }
        #endregion methods
    }
}