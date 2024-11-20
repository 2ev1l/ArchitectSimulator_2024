using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public abstract class PremiseInfo : DBInfo, INameHandler, IPreviewHandler
    {
        #region fields & properties
        public Sprite PreviewSprite => previewSprite;
        [SerializeField] private Sprite previewSprite;
        public LanguageInfo NameInfo => nameInfo;
        [SerializeField] private LanguageInfo nameInfo;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}