using Game.DataBase;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class PlayerTaskData : TaskData<PlayerTaskInfo>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override PlayerTaskInfo GetInfo()
        {
            return DB.Instance.PlayerTaskInfo[Id].Data;
        }
        public PlayerTaskData(int id) : base(id) { }
        public PlayerTaskData(int id, int duration, int currentMonth) : base(id, duration, currentMonth) { }
        #endregion methods
    }
}