using EditorCustom.Attributes;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;

namespace Game.UI.Overlay.Computer.Browser.Company
{
    public class ConstructionResourceWarehouseItem : ResourceWarehouseItem<ConstructionResourceData>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void RemoveResourceFromWarehouse(ConstructionResourceData resource, int count)
        {
            Warehouse.RemoveConstructionResource(resource, count);
        }
        #endregion methods
    }
}