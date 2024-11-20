using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class RatingData
    {
        #region fields & properties
        /// <summary>
        /// <see cref="RangedValue.OnValueChanged"/> 
        /// </summary>
        public UnityAction<int, int> OnValueChanged
        {
            get => rating.OnValueChanged;
            set => rating.OnValueChanged = value;
        }
        public int Value => rating.Value;
        [SerializeField] private RangedValue rating = new(0, new(0, 100));
        #endregion fields & properties

        #region methods
        public bool TryIncreaseValue(int amount) => rating.TryIncreaseValue(amount);
        public bool TryDecreaseValue(int amount) => rating.TryDecreaseValue(amount);
        #endregion methods
    }
}