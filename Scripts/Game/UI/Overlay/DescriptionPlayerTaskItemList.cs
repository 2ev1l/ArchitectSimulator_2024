using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Overlay
{
    public class DescriptionPlayerTaskItemList : DescriptionTaskItemList<PlayerTaskData, PlayerTaskInfo>
    {
        #region fields & properties
        protected override TasksData<PlayerTaskData, PlayerTaskInfo> TasksData => GameData.Data.PlayerData.Tasks;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}