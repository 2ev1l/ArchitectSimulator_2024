using EditorCustom.Attributes;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class CompanyDataChecker : ResultChecker
    {
        #region fields & properties
        private static CompanyData CompanyData => GameData.Data.CompanyData;
        [SerializeField] private bool companyMustBeCreated = true;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            CompanyData.OnCreated += Check;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            CompanyData.OnCreated -= Check;
        }
        public override bool GetResult()
        {
            if (companyMustBeCreated)
                return CompanyData.IsCreated;
            else
                return !CompanyData.IsCreated;
        }
        #endregion methods
    }
}