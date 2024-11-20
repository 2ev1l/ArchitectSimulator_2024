using Game.DataBase;
using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.Collections
{
    public class BlueprintInfoItem : DBItem<BlueprintInfo>
    {
        #region fields & properties
        [SerializeField] private BuildingDataItem buildingDataItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(BlueprintInfo param)
        {
            base.OnListUpdate(param);
            buildingDataItem.OnListUpdate(param.BuildingInfo);
        }
        #endregion methods
    }
}