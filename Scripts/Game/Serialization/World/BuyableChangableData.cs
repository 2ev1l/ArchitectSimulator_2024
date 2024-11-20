using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class BuyableChangableData<T> : ChangableInfoData<T> where T : DBInfo
    {
        #region fields & properties
        public BuyableObject BuyableInfo
        {
            get
            {
                if (buyableInfo == null || buyableInfo.Id != Id)
                {
                    try { buyableInfo = GetBuyableInfo(); }
                    catch { buyableInfo = null; }
                }
                return buyableInfo;
            }
        }
        [System.NonSerialized] private BuyableObject buyableInfo = null;
        #endregion fields & properties

        #region methods
        protected abstract BuyableObject GetBuyableInfo();
        protected override void OnInfoReplaced()
        {
            _ = BuyableInfo;
        }
        protected BuyableChangableData(int id) : base(id) { }
        #endregion methods
    }
}