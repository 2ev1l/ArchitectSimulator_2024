using UnityEngine;
using UnityEngine.UI;
using Universal.Collections.Generic.Filters;

namespace Universal.Collections.Filters
{
    public abstract class ImageFilterItem<T> : VirtualFilterItem<T>
    {
        #region fields & properties
        protected Image Image => image;
        [SerializeField] private Image image;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}