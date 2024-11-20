using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class SelectableBuilderItem : SelectableItem<BuilderData>
    {
        #region fields & properties
        [SerializeField] private BuilderItem builderItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(BuilderData param)
        {
            base.OnListUpdate(param);
            builderItem.OnListUpdate(param);
        }
        #endregion methods
    }
}