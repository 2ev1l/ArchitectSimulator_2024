using Game.DataBase;
using Universal.Collections.Filters;

namespace Game.UI.Overlay.Computer
{
    public class ResourceColorFilterItem : ImageFilterItem<ResourceColor>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            Image.color = Value.ToColorRGB();
        }
        #endregion methods
    }
}