using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class PlayerTaskInfo : TaskInfo
    {
        #region fields & properties
        public IReadOnlyList<Sprite> SpritesInfo => spritesInfo;
        [SerializeField] private Sprite[] spritesInfo;

        public IReadOnlyList<int> NextTasksTrigger => nextTasksTrigger;
        [Title("Base Triggers")][SerializeField] private int[] nextTasksTrigger = new int[0];
        public IReadOnlyList<int> StartSubtitlesTrigger => startSubtitlesTrigger;
        [SerializeField] private int[] startSubtitlesTrigger = new int[0];
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}