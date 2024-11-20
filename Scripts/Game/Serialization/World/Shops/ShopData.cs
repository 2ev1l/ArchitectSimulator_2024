using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class ShopData<T> : IMonthUpdatable where T : ShopItemData, ICloneable<T>
    {
        #region fields & properties
        public UnityAction OnDataChanged;
        public IReadOnlyList<T> Items => items;
        [SerializeField] protected List<T> items = new();
        #endregion fields & properties

        #region methods
        public void OnMonthUpdate(MonthData currentMonth)
        {
            GenerateNewData();
        }
        public virtual void GenerateNewData()
        {
            items = GetNewData();
            OnDataChanged?.Invoke();
        }
        protected abstract List<T> GetNewData();
        #endregion methods
    }
}