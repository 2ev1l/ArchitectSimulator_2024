using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class HouseData : RealEstateData
    {
        #region fields & properties
        public override string BillDescription => LanguageInfo.GetTextByType(TextType.Game, 296);
        public int CurrentPeopleCount => currentPeopleCount;
        [SerializeField][Min(1)] private int currentPeopleCount = 1;
        #endregion fields & properties

        #region methods
        public override bool CanReplaceInfo(int newInfoId)
        {
            if (!base.CanReplaceInfo(newInfoId)) return false;
            if (newInfoId < 0) return false;

            HouseInfo newInfo = (HouseInfo)GetInfo(newInfoId);
            return newInfo.MaxPeople >= currentPeopleCount;
        }
        public bool CanAddPeople() => currentPeopleCount < ((HouseInfo)Info).MaxPeople;
        public bool TryAddPeople()
        {
            if (!CanAddPeople()) return false;
            currentPeopleCount++;
            return true;
        }
        public bool TryRemovePeople()
        {
            if (currentPeopleCount <= 1) return false;
            currentPeopleCount--;
            return true;
        }
        protected override RentableRealEstate GetRentableInfo(int baseInfoReferenceId) => DB.Instance.RentableHouseInfo.Find(x => x.Data.RealEstateInfo.Id == baseInfoReferenceId).Data;
        protected override RealEstateInfo GetInfo(int id) => DB.Instance.HouseInfo[id].Data;
        public HouseData(int id) : base(id) { }
        #endregion methods
    }
}