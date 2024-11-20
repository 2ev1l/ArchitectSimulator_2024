using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class OfficeDataChecker : ResultChecker
    {
        #region fields & properties
        private static OfficeData OfficeData => GameData.Data.CompanyData.OfficeData;
        [SerializeField] private bool officeMustExist = true;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            OfficeData.OnInfoChanged -= Check;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            OfficeData.OnInfoChanged -= Check;
        }
        public override bool GetResult()
        {
            if (officeMustExist)
                return OfficeData.Id > -1;
            else
                return OfficeData.Id < 0;
        }
        #endregion methods
    }
}