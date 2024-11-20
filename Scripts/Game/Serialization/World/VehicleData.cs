using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class VehicleData : BuyableChangableData<VehicleInfo>, IMoodScaleHandler
    {
        #region fields & properties
        public float MoodScale => Id < 0 ? 1 : Info.MoodScale;
        #endregion fields & properties

        #region methods
        protected override void OnInfoReplaced()
        {
            base.OnInfoReplaced();
        }
        protected override VehicleInfo GetInfo()
        {
            return DB.Instance.VehicleInfo[Id].Data;
        }

        protected override BuyableObject GetBuyableInfo()
        {
            int id = Id;
            return DB.Instance.BuyableVehicleInfo.Find(x => x.Id == id).Data;
        }

        public VehicleData(int id) : base(id) { }
        #endregion methods
    }
}