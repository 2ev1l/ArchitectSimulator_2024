using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public abstract class BuyableResource : BuyableObject, IMinimalRatingHandler
    {
        #region fields & properties
        public int MinRating => minRating;
        [SerializeField][Range(0, 99)] private int minRating = 0;
        public abstract ResourceInfo ResourceInfo { get; }
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}