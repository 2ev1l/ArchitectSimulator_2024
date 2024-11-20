using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections.Generic.Filters;
using Universal.Core;

namespace Game.UI.Overlay.Computer
{
    public class ConstructionResourceBuildingFloorFilter : VirtualDropdownFilter<BuildingFloor>, ISmartFilter<ConstructionResourceInfo>
    {
        #region fields & properties
        public VirtualFilter VirtualFilter => this;
        private IEnumerable<BuildingFloor> appliedFloors;
        private readonly List<BuildingFloor> appliedFlags = new();
        #endregion fields & properties

        #region methods
        public void UpdateFilterData()
        {
            appliedFloors = GetEnabledFilters().Select(x => x.Value);
        }
        public bool FilterItem(ConstructionResourceInfo item)
        {
            item.BuildingFloor.ToFlagList(appliedFlags);
            return appliedFlags.Any(x => appliedFloors.Any(y => y == x));
        }
        #endregion methods
    }
}