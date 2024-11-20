using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections.Filters;
using Universal.Collections.Generic.Filters;
using Universal.Core;

namespace Game.UI.Overlay.Computer
{
    [System.Serializable]
    public class ConstructionResourceGroupFilter : VirtualDropdownFilter<ConstructionResourceGroupType>, ISmartFilter<ConstructionResourceInfo>
    {
        #region fields & properties
        public VirtualFilter VirtualFilter => this;
        private IEnumerable<ConstructionResourceGroupType> appliedTypes;
        private readonly List<ConstructionResourceGroupType> appliedFlags = new();
        #endregion fields & properties

        #region methods
        public void UpdateFilterData()
        {
            appliedTypes = GetEnabledFilters().Select(x => x.Value);
        }
        public bool FilterItem(ConstructionResourceInfo item)
        {
            var group = item.RelatedGroup;
            if (group == null)
            {
                return false;
            }
            group.GroupTypes.ToFlagList(appliedFlags);
            return appliedFlags.Any(x => appliedTypes.Any(y => y == x));
        }
        #endregion methods
    }
}