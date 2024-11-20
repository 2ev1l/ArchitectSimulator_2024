using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class CartData<T> where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        public UnityAction OnCartBought;
        private Wallet PlayerWallet => GameData.Data.PlayerData.Wallet;
        public UnityAction<CountableItem<T>> OnItemAdded
        {
            get => cart.OnItemAdded;
            set => cart.OnItemAdded = value;
        }
        public UnityAction<CountableItem<T>> OnItemRemoved
        {
            get => cart.OnItemRemoved;
            set => cart.OnItemRemoved = value;
        }
        public IReadOnlyList<CountableItem<T>> Items => cart.Items;
        [SerializeField] private CountableItemList<T> cart = new();
        #endregion fields & properties

        #region methods
        public void Clear() => cart.Clear();
        
        public virtual void Add(T shopItem, int count)
        {
            if (count <= 0) return;
            T clone = shopItem.Clone();
            cart.AddItem(clone, x => CartItemsPredicate(x, clone), count);
        }
        public void Remove(T cartItem, int count)
        {
            if (count <= 0) return;
            cart.RemoveItem(x => CartItemsPredicate(x, cartItem), ref count);
        }
        /// <summary>
        /// For default, compares only for id
        /// </summary>
        /// <param name="existCartItem"></param>
        /// <param name="currentItem"></param>
        /// <returns></returns>
        protected virtual bool CartItemsPredicate(CountableItem<T> existCartItem, T currentItem)
        {
            return existCartItem.Item.Id == currentItem.Id;
        }
        public virtual bool CanPurchaseCart()
        {
            if (Items.Count == 0) return false;
            int moneySum = GetCartSum();
            return PlayerWallet.CanDecreaseValue(moneySum);
        }
        public virtual int GetCartSum()
        {
            int sum = 0;
            foreach (var item in cart.Items)
            {
                sum += item.Item.FinalPrice * item.Count;
            }
            return sum;
        }

        /// <summary>
        /// You need to check manually for <see cref="CanPurchaseCart"/> <br></br>
        /// Clears all cart items
        /// </summary>
        public virtual void PurchaseCart()
        {
            foreach (var el in cart.Items)
            {
                el.Item.OnPurchase(el.Count);
            }
            PlayerWallet.TryDecreaseValue(GetCartSum());
            cart.Clear();
            OnCartBought?.Invoke();
        }
        #endregion methods
    }
}