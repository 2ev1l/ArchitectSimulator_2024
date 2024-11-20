namespace Universal.Collections.Filters
{
    public class InputIntFilter : InputFilter<int>
    {
        #region fields & properties
        public override int Data => System.Convert.ToInt32(InputField.text);
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}