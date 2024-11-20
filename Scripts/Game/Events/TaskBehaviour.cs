using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class TaskBehaviour : MonoBehaviour
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public void TryStartTask(int id) => GameData.Data.PlayerData.Tasks.TryStartTask(id);
        public void TryCompleteTask(int id) => GameData.Data.PlayerData.Tasks.TryCompleteTask(id);
        #endregion methods
    }
}