using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public struct Reward
    {
        #region fields & properties
        public readonly int Value => value;
        [SerializeField][Min(0)] private int value;
        //Required for simple serialization in scriptable objects, else should made inheritance
        public readonly RewardType Type => type;
        [SerializeField] private RewardType type;
        #endregion fields & properties

        #region methods
        public readonly string GetLanguage() => $"{type.GetLanguage()}: {type.GetValueText(value)}";
        public Reward(int value, RewardType type)
        {
            this.value = value;
            this.type = type;
        }
        #endregion methods
    }
}