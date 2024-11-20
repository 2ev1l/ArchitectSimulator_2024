using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class TaskInfo : DBInfo, INameHandler, IDescriptionHandler
    {
        #region fields & properties
        public LanguageInfo NameInfo => nameInfo;
        [Title("UI")][SerializeField] private LanguageInfo nameInfo = new(0, TextType.Task);
        public LanguageInfo DescriptionInfo => descriptionInfo;
        [SerializeField] private LanguageInfo descriptionInfo = new(0, TextType.Task);
        /// <summary>
        /// 0 equals Infinity
        /// </summary>
        public int MonthDuration => monthDuration;
        [SerializeField][Min(0)] private int monthDuration = 1;
        public RewardInfo RewardInfo => rewardInfo;
        [Title("Settings")][SerializeField] private RewardInfo rewardInfo;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}