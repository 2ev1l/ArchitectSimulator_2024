using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ChangableInfoBuyableItemData<Buyable, Info> : BuyableObjectItemData<Buyable>
        where Buyable : BuyableObject
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
        protected ChangableInfoBuyableItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}