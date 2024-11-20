using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class DescriptionUnfinishedBlueprintItem : DescriptionItem<BlueprintData>
    {
        #region fields & properties
        [SerializeField] private UnfinishedBlueprintItem unfinishedBlueprintItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(BlueprintData param)
        {
            base.OnListUpdate(param);
            unfinishedBlueprintItem.OnListUpdate(param);
        }
        protected override void SendPanelRequest()
        {
            base.SendPanelRequest();
            BlueprintEditor.Instance.TryLoadData(Context.Name);
        }
        protected override void SendNullRequest()
        {
            base.SendNullRequest();
            BlueprintEditor.Instance.UnloadData();
        }
        #endregion methods
    }
}