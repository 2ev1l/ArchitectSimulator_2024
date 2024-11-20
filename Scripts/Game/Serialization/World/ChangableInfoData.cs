using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ChangableInfoData<T> where T : DBInfo
    {
        #region fields & properties
        public UnityAction OnInfoChanged;
        public int Id => id;
        [SerializeField][Min(-1)] private int id = 0;
        /// <exception cref="System.NullReferenceException"></exception>
        public T Info
        {
            get
            {
                if (info == null || info.Id != Id)
                {
                    try { info = GetInfo(); }
                    catch { info = null; }
                }
                return info;
            }
        }
        [System.NonSerialized] private T info = null;
        #endregion fields & properties

        #region methods
        public virtual bool CanReplaceInfo(int newInfoId)
        {
            return newInfoId >= -1;
        }
        public bool TryReplaceInfo(int newInfoId)
        {
            if (!CanReplaceInfo(newInfoId)) return false;
            id = newInfoId;
            _ = Info;
            OnInfoReplaced();
            OnInfoChanged?.Invoke();
            return true;
        }
        /// <summary>
        /// Invokes just before the action
        /// </summary>
        protected virtual void OnInfoReplaced() { }
        /// <summary>
        /// Override this instead of Info property
        /// </summary>
        /// <returns></returns>
        protected abstract T GetInfo();

        public ChangableInfoData(int id)
        {
            id = Mathf.Max(id, -1);
            this.id = id;
        }
        #endregion methods
    }
}