using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    [System.Serializable]
    public class FoodData : IMonthUpdatable
    {
        #region fields & properties
        /// <summary>
        /// Limit after which game may be ended
        /// </summary>
        public const int NEGATIVE_SATURATION_LIMIT = 180;
        /// <summary>
        /// <see cref="{T0}"/> - totalSaturation <br></br>
        /// <see cref="{T1}"/> - increasedSaturation
        /// </summary>
        public UnityAction<int, int> OnSaturationIncreased;
        public bool CanIncreaseSaturation => canIncreaseSaturation;
        [SerializeField] private bool canIncreaseSaturation = true;
        /// <summary>
        /// Total saturation that stored over the game
        /// </summary>
        public int TotalSaturation => totalSaturation;
        [SerializeField][Min(0)] private int totalSaturation = 0;
        /// <summary>
        /// Saturation that should be fullfilled
        /// </summary>
        public int NegativeSaturation => negativeSaturation;
        [SerializeField][Min(0)] private int negativeSaturation = 0;
        #endregion fields & properties

        #region methods
        public bool TryIncreaseSaturation(int value)
        {
            if (value <= 0) return false;
            if (!canIncreaseSaturation) return false;
            totalSaturation += value;
            negativeSaturation = Mathf.Max(negativeSaturation - value, 0);
            canIncreaseSaturation = false;
            OnSaturationIncreased?.Invoke(totalSaturation, value);
            return true;
        }
        private void SetNegativeSaturation(MonthData monthData)
        {
            int negativeIncrease = monthData.CurrentMonth switch
            {
                int i when i < 5 => 10,
                int i when i < 12 => 15,
                int i when i < 23 => 20,
                int i when i < 31 => 30,
                _ => 35
            };
            negativeIncrease = Mathf.RoundToInt(negativeIncrease * Random.Range(0.7f, 1f));
            negativeSaturation += negativeIncrease;
        }
        public void OnMonthUpdate(MonthData monthData)
        {
            canIncreaseSaturation = true;
            SetNegativeSaturation(monthData);
        }
        #endregion methods
    }
}