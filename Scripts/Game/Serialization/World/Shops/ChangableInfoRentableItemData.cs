using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ChangableInfoRentableItemData<Rentable, Info> : RentableObjectItemData<Rentable>
        where Rentable : RentableObject
        where Info : DBInfo
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            base.OnPurchase(count);
            GetChangableInfo().TryReplaceInfo(Id);
        }
        protected abstract ChangableInfoData<Info> GetChangableInfo();
        protected ChangableInfoRentableItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}