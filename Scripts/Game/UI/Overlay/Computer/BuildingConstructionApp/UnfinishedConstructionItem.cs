using Game.Serialization.World;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class UnfinishedConstructionItem : DescriptionItem<ConstructionData>
    {
        #region fields & properties
        [SerializeField] private ConstructionDataItem constructionDataItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(ConstructionData param)
        {
            base.OnListUpdate(param);
            constructionDataItem.OnListUpdate(param);
        }
        #endregion methods
    }
}