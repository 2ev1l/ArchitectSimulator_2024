using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class WarehouseDataChecker : ResultChecker
    {
        #region fields & properties
        private static WarehouseData WarehouseData => GameData.Data.CompanyData.WarehouseData;
        [SerializeField] private bool warehouseMustExist = true;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            WarehouseData.OnInfoChanged -= Check;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            WarehouseData.OnInfoChanged -= Check;
        }
        public override bool GetResult()
        {
            if (warehouseMustExist)
                return WarehouseData.Id > -1;
            else
                return WarehouseData.Id < 0;
        }
        #endregion methods
    }
}