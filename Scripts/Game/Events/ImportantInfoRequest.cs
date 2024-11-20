using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Events;

namespace Game.Events
{
    [System.Serializable]
    public class ImportantInfoRequest : ExecutableRequest
    {
        #region fields & properties
        public string Text => text;
        [SerializeField] private string text;
        #endregion fields & properties

        #region methods
        public override void Close()
        {

        }

        public ImportantInfoRequest(string text)
        {
            this.text = text;
        }
        #endregion methods
    }
}