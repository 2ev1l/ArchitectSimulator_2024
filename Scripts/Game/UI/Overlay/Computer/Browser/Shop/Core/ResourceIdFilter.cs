using Game.DataBase;
using Game.Serialization.World;
using Universal.Collections.Filters;
using Universal.Collections.Generic.Filters;

namespace Game.UI.Overlay.Computer.Browser.Shop
{
    public class ResourceIdFilter : InputIntFilter, ISmartFilter<ResourceInfo>
    {
        #region fields & properties
        public VirtualFilter VirtualFilter => this;
        private int id = 0;
        #endregion fields & properties

        #region methods
        public void UpdateFilterData()
        {
            id = Data;
        }
        public bool FilterItem(ResourceInfo item)
        {
            return item.Id == id;
        }
        #endregion methods
    }
}