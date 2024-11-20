using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Overlay
{
    public class DescriptionPlayerTaskPanel : DescriptionTaskPanel<PlayerTaskData, PlayerTaskInfo>
    {
        #region fields & properties
        [SerializeField] private TaskPreviewItemList previewItemList;
        [SerializeField] private GameObject previewGroup;
        #endregion fields & properties

        #region methods
        protected override void OnUpdateUI()
        {
            base.OnUpdateUI();
            previewItemList.TaskData = Data;
            previewItemList.UpdateListData();
            previewGroup.SetActive(Data.Info.SpritesInfo.Count() != 0);
        }
        #endregion methods
    }
}