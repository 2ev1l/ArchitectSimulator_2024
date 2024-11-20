using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    public enum ConstructionSubtype
    {
        Base,
        CornerIn,
        CornerOut,
        Door,
        Window,
        Staircase
    }
    #endregion enum

    public static class ConstructionSubtypeExtension
    {
        #region methods
        public static string ToLanguage(this ConstructionSubtype subtype) => subtype switch
        {
            ConstructionSubtype.Base => LanguageInfo.GetTextByType(TextType.Resource, 0),
            ConstructionSubtype.CornerIn => LanguageInfo.GetTextByType(TextType.Resource, 12),
            ConstructionSubtype.CornerOut => LanguageInfo.GetTextByType(TextType.Resource, 13),
            ConstructionSubtype.Door => LanguageInfo.GetTextByType(TextType.Resource, 2),
            ConstructionSubtype.Window => LanguageInfo.GetTextByType(TextType.Resource, 1),
            ConstructionSubtype.Staircase => LanguageInfo.GetTextByType(TextType.Resource, 20),
            _ => throw new System.NotImplementedException($"language for {subtype}"),
        };
        #endregion methods
    }
}