using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.Events
{
    [System.Serializable]
    public abstract class PopupRequest : ExecutableRequest
    {
        #region fields & properties
        public Sprite IndicatorSprite => indicatorSprite;
        [SerializeField] private Sprite indicatorSprite;
        #endregion fields & properties

        #region methods
        public abstract Color GetColor();
        public abstract string GetText();
        public override void Close()
        {

        }
        #endregion methods
    }
}