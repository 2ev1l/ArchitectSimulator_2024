using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    [System.Serializable]
    public class StaticTextPopupRequest : PopupRequest
    {
        #region fields & properties
        public string Text
        {
            get => text;
            set => text = value;
        }
        private string text;
        #endregion fields & properties

        #region methods
        public override string GetText() => text;
        public override Color GetColor() => Color.white;
        #endregion methods
    }
}