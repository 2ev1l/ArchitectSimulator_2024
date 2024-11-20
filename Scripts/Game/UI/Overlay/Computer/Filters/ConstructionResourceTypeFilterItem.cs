using Game.DataBase;
using Universal.Collections.Filters;

namespace Game.UI.Overlay.Computer
{
    public class ConstructionResourceTypeFilterItem : TextFilterItem<ConstructionType>
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            Text.text = Value.ToLanguage();
        }
        #endregion methods
    }
}