using EditorCustom.Attributes;
using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    /// <summary>
    /// Represents mood as percent int [0..100%]
    /// </summary>
    [System.Serializable]
    public class MoodData : IMonthUpdatable
    {
        #region fields & properties
        private static readonly int moodIncreaseValue = 25;
        /// <summary>
        /// <see cref="RangedValue.OnValueChanged"/>
        /// </summary>
        public UnityAction<int, int> OnValueChanged
        {
            get => mood.OnValueChanged;
            set => mood.OnValueChanged = value;
        }
        public int Value => mood.Value;
        [SerializeField] private RangedValue mood = new(100, new(0, 100));
        #endregion fields & properties

        #region methods
        public void OnMonthUpdate(MonthData currentMonth)
        {
            UpdateMood(currentMonth);
        }

        /// <summary>
        /// Increases until max.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>False if value less than 0</returns>
        public bool TryIncreaseValue(int value) => mood.TryIncreaseValue(Mathf.Clamp(value, 0, mood.MaxChangesLimit));
        /// <summary>
        /// Decreases until max.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>False if value less than 0</returns>
        public bool TryDecreaseValue(int value) => mood.TryDecreaseValue(Mathf.Clamp(value, 0, mood.MinChangesLimit));
        private float CalculateMoodIncreaseScale()
        {
            float scale = 1f;
            IReadOnlyList<IMoodScaleHandler> moodScalers = GameData.Data.GetMoodScalers();
            foreach (IMoodScaleHandler el in moodScalers)
            {
                float elMood = (1 + el.MoodScale) / 2;
                scale *= elMood;
            }
            return scale;
        }
        private float CalculateMoodDecreaseScale(MonthData currentMonth)
        {
            float scale = 1f;
            int minMonthDecrease = 11;
            scale *= Mathf.Clamp01(currentMonth.CurrentMonth / (float)minMonthDecrease);

            scale = Mathf.Clamp01(scale);
            return scale;
        }

        private void UpdateMood(MonthData currentMonth)
        {
            int timeSpentPercent = 0;
            float moodIncreaseScale = CalculateMoodIncreaseScale();
            float moodDecreaseScale = CalculateMoodDecreaseScale(currentMonth);
            if (currentMonth.StartStatistic.Time != 0)
                timeSpentPercent = (int)(100 * ((float)Mathf.Abs(currentMonth.GainStatistic.Time) / currentMonth.StartStatistic.Time));

            int timeSpentPercentForDecrease = (int)Mathf.Clamp(15.7f * moodIncreaseScale, 7, 70);
            if (timeSpentPercent >= timeSpentPercentForDecrease)
            {
                float overworkPercent = 1f - (timeSpentPercentForDecrease / (float)timeSpentPercent);
                int moodDecrease = Mathf.Clamp((int)(overworkPercent * moodDecreaseScale * 29), 0, mood.MinChangesLimit);
                mood.TryDecreaseValue(moodDecrease);
            }
            else
            {
                float percentMoodIncrease = 1f - (timeSpentPercent / (float)timeSpentPercentForDecrease);
                percentMoodIncrease = Mathf.Pow(percentMoodIncrease, 1.5f);
                int moodIncrease = Mathf.Clamp((int)(moodIncreaseValue * percentMoodIncrease), 0, mood.MaxChangesLimit);
                mood.TryIncreaseValue(moodIncrease);
            }
        }
        #endregion methods
    }
}