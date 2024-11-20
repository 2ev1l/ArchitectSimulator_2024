using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class TaskData<T> where T : TaskInfo
    {
        #region fields & properties
        public int Id => id;
        [SerializeField] private int id;
        [SerializeField][Min(0)] private int expirationMonth;
        public int MonthLeft => expirationMonth == 0 ? 999 : expirationMonth - CurrentMonth;
        protected static int CurrentMonth => GameData.Data.PlayerData.MonthData.CurrentMonth;

        public T Info
        {
            get
            {
                if (info == null)
                {
                    try { info = GetInfo(); }
                    catch { info = null; }
                }
                return info;
            }
        }
        [System.NonSerialized] private T info;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// If expiration month equals or greater current month
        /// </summary>
        internal bool IsExpired() => (CurrentMonth >= expirationMonth && expirationMonth != 0);
        public bool IsLastMonthBeforeExpiration() => MonthLeft == 1;
        protected abstract T GetInfo();
        /// <summary>
        /// Collects data just by id. Use this only when game is initialized
        /// </summary>
        /// <param name="id"></param>
        public TaskData(int id)
        {
            this.id = id;
            int duration = Info.MonthDuration;
            this.expirationMonth = duration == 0 ? 0 : CurrentMonth + duration;
        }
        /// <summary>
        /// You can use this even when game is not running
        /// </summary>
        /// <param name="id"></param>
        /// <param name="duration"></param>
        public TaskData(int id, int duration, int currentMonth)
        {
            this.id = id;
            this.expirationMonth = duration == 0 ? 0 : currentMonth + duration;
        }
        #endregion methods
    }
}