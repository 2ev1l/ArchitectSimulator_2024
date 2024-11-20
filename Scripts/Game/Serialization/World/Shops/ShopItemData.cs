using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ShopItemData
    {
        #region fields & properties
        public int Id => id;
        [SerializeField][Min(0)] private int id;
        public int FinalPrice => CustomMath.Multiply(StartPrice, 100 - discount);
        public int StartPrice => startPrice;
        [SerializeField][Min(1)] private int startPrice;
        public bool HasDiscount => discount > 0;
        public int Discount => discount;
        [SerializeField][Min(0)] private int discount = 0;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// </summary>
        /// <param name="count"></param>
        public virtual void OnPurchase(int count)
        {

        }
        public ShopItemData(int id, int startPrice, int discount)
        {
            this.id = Mathf.Max(id, 0);
            this.startPrice = Mathf.Max(startPrice, 1);
            this.discount = Mathf.Max(discount, 0);
        }
        #endregion methods
    }
}