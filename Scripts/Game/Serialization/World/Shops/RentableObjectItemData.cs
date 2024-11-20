using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RentableObjectItemData<T> : BuyableObjectItemData<T>, ISingleShopItem where T : RentableObject
    {
        #region fields & properties
        public bool IsOwned => isOwned;
        [SerializeField] private bool isOwned = false;
        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            base.OnPurchase(count);
            MakeOwned();
        }
        public void MakeOwned()
        {
            isOwned = true;
        }
        protected RentableObjectItemData(int id, int startPrice, int discount) : base(id, startPrice, discount) { }
        #endregion methods
    }
}