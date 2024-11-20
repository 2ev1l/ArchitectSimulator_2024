using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RecruitEmployeeData<T> : ShopItemData, ISingleShopItem where T : EmployeeData
    {
        #region fields & properties
        public T Employee => employee;
        [SerializeField] private T employee;
        public bool IsOwned => isOwned;
        [SerializeField] private bool isOwned = false;
        #endregion fields & properties

        #region methods
        public override void OnPurchase(int count)
        {
            base.OnPurchase(count);
            MakeOwned();
            Hire();
        }
        public void MakeOwned()
        {
            isOwned = true;
        }
        protected abstract void Hire();
        public RecruitEmployeeData(int id, int startPrice, int discount, T employee) : base(id, startPrice, discount)
        {
            this.employee = employee;
        }
        #endregion methods
    }
}