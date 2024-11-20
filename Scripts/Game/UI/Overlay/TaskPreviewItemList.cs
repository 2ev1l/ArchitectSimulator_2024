using Game.Serialization.World;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Overlay
{
    public class TaskPreviewItemList : InfinityItemListBase<TaskPreviewItem, Sprite>
    {
        #region fields & properties
        public PlayerTaskData TaskData
        {
            get => taskData;
            set => taskData = value;
        }
        private PlayerTaskData taskData;
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            if (taskData == null) return;
            ItemList.UpdateListDefault(taskData.Info.SpritesInfo, x => x);
        }
        #endregion methods
    }
}