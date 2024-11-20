using TMPro;
using UnityEngine;
using Universal.Collections.Generic.Filters;

namespace Universal.Collections.Filters
{
    public abstract class TextFilterItem<T> : VirtualFilterItem<T>
    {
        #region fields & properties
        protected TextMeshProUGUI Text => text;
        [SerializeField] private TextMeshProUGUI text;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}