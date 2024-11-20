using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Overlay.Computer.Collections
{
    [System.Serializable]
    public class MinimalRatingExposer
    {
        #region fields & properties
        [SerializeField] private Slider ratingSlider;
        #endregion fields & properties

        #region methods
        public void Expose(IMinimalRatingHandler minimalRatingHandler)
        {
            if (ratingSlider == null) return;
            ratingSlider.value = minimalRatingHandler.MinRating;
        }
        #endregion methods
    }
}