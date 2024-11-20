using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class RealEstateInfo : DBInfo, INameHandler, IPreviewHandler, IMoodScaleHandler
    {
        #region fields & properties
        public Sprite PreviewSprite => previewSprite;
        [SerializeField] private Sprite previewSprite;
        public LanguageInfo NameInfo => nameInfo;
        [SerializeField] private LanguageInfo nameInfo;
        public float MoodScale => moodScale;
        [SerializeField][Min(0.1f)] private float moodScale = 1;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}