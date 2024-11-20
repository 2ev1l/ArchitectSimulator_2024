using Game.DataBase;
using Universal.Collections.Filters;

namespace Game.UI.Overlay.Computer
{
    public class ConstructionResourceSubtypeFilterItem : TextFilterItem<ConstructionSubtype>
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