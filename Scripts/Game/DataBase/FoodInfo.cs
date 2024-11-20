using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class FoodInfo : DBInfo, INameHandler, IPreviewHandler
    {
        #region fields & properties
        public Sprite PreviewSprite => previewSprite;
        [SerializeField] private Sprite previewSprite;
        public LanguageInfo NameInfo => nameInfo;
        [SerializeField] private LanguageInfo nameInfo;
        public int Saturation => saturation;
        [SerializeField][Range(1, 100)] private int saturation = 1;
        public int MoodChange => moodChange;
        [SerializeField][Range(-10, 10)] private int moodChange = 1;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}