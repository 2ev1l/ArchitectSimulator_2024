using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RentablePremiseData : PremiseData, IBillPayable
    {
        #region fields & properties
        public bool CanAddBill => Id > -1 && RentableInfo != null;
        public virtual int BillPaymentAmount => RentableInfo.RentPrice;
        public abstract string BillDescription { get; }

        /// <exception cref="System.NullReferenceException"></exception>
        public RentablePremise RentableInfo
        {
            get
            {
                if (rentableInfo == null || rentableInfo.Id != Id)
                {
                    try { rentableInfo = GetRentablePremiseInfo(); }
                    catch { rentableInfo = null; }
                }
                return rentableInfo;
            }
        }
        [System.NonSerialized] private RentablePremise rentableInfo = null;
        #endregion fields & properties

        #region methods
        protected override void OnInfoReplaced()
        {
            _ = RentableInfo;
        }
        protected abstract RentablePremise GetRentablePremiseInfo();
        protected override PremiseInfo GetInfo() => RentableInfo.PremiseInfo;
        protected RentablePremiseData(int id) : base(id) { }
        #endregion methods
    }
}