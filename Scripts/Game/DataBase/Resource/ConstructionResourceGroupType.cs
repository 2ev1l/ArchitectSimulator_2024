using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    [System.Flags]
    public enum ConstructionResourceGroupType
    {
        //none = 0
        Sturdy = 1,
        Unstable = 2,
        Usual = 4,
        Insulated = 8
    }
    #endregion enum

    public static class ConstructionResourceGroupTypeExtension
    {
        #region methods
        /// <summary>
        /// Doesn't work with flags <br></br>
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string ToLanguage(this ConstructionResourceGroupType gt) => gt switch
        {
            0 => LanguageInfo.GetTextByType(TextType.Resource, 24),
            ConstructionResourceGroupType.Sturdy => LanguageInfo.GetTextByType(TextType.Resource, 37),
            ConstructionResourceGroupType.Unstable => LanguageInfo.GetTextByType(TextType.Resource, 38),
            ConstructionResourceGroupType.Usual => LanguageInfo.GetTextByType(TextType.Resource, 50),
            ConstructionResourceGroupType.Insulated => LanguageInfo.GetTextByType(TextType.Resource, 51),

            _ => throw new System.NotImplementedException($"langauge for {gt}"),
        };
        #endregion methods
    }
}