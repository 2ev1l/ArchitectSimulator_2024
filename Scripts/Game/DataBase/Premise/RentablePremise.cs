using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public abstract class RentablePremise : RentableObject, IMinimalRatingHandler
    {
        #region fields & properties
        public abstract PremiseInfo PremiseInfo { get; }
        public int MinRating => minRating;
        [SerializeField][Range(0, 99)] private int minRating = 0;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}