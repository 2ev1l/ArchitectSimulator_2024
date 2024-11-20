using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public abstract class RealEstateData : ChangableInfoData<RealEstateInfo>, IBillPayable, IMoodScaleHandler
    {
        #region fields & properties
        public float MoodScale => Info == null ? 1f : Info.MoodScale;
        public bool CanAddBill => Id > -1;
        public int BillPaymentAmount => RentableInfo.RentPrice;
        public abstract string BillDescription { get; }

        /// <exception cref="System.NullReferenceException"></exception>
        public RentableRealEstate RentableInfo
        {
            get
            {
                if (rentableInfo == null || rentableInfo.Id != Id)
                {
                    try { rentableInfo = GetRentableInfo(); }
                    catch { rentableInfo = null; }
                }
                return rentableInfo;
            }
        }
        [System.NonSerialized] private RentableRealEstate rentableInfo = null;
        #endregion fields & properties

        #region methods
        protected override RealEstateInfo GetInfo() => GetInfo(Id);
        protected abstract RealEstateInfo GetInfo(int id);
        private RentableRealEstate GetRentableInfo() => GetRentableInfo(Id);
        protected abstract RentableRealEstate GetRentableInfo(int baseInfoReferenceId);

        protected override void OnInfoReplaced()
        {
            base.OnInfoReplaced();
            _ = RentableInfo;
        }
        public RealEstateData(int id) : base(id) { }
        #endregion methods
    }
}