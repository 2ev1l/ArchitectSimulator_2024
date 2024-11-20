using Game.DataBase;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    public class RentablePremiseItem : RentableObjectItem<RentablePremise>
    {
        #region fields & properties
        [SerializeField] private PremiseItem premiseItem;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(RentablePremise param)
        {
            base.OnListUpdate(param);
            premiseItem.OnListUpdate(param.PremiseInfo);
        }
        #endregion methods
    }
}