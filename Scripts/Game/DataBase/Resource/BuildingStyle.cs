using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    [System.Flags]
    public enum BuildingStyle
    {
        //none = 0
        American = 1,
        Wooden = 2,
        Industrial = 4,
        Ancient = 8,
        City = 16,
        Medieval = 32,
        Modern = 64,
        Neoclassical = 128,
        Scandinavian = 256,
        Renaissance = 512,
    }
    #endregion enum

    public static class BuildingStyleExtension
    {
        #region methods
        /// <summary>
        /// Doesn't work with flags <br></br>
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string ToLanguage(this BuildingStyle bs) => bs switch
        {
            0 => LanguageInfo.GetTextByType(TextType.Resource, 24),
            BuildingStyle.American => LanguageInfo.GetTextByType(TextType.Resource, 7),
            BuildingStyle.Wooden => LanguageInfo.GetTextByType(TextType.Resource, 26),
            BuildingStyle.Industrial => LanguageInfo.GetTextByType(TextType.Resource, 52),
            BuildingStyle.Ancient => LanguageInfo.GetTextByType(TextType.Resource, 55),
            BuildingStyle.City => LanguageInfo.GetTextByType(TextType.Resource, 57),
            BuildingStyle.Medieval => LanguageInfo.GetTextByType(TextType.Resource, 65),
            BuildingStyle.Modern => LanguageInfo.GetTextByType(TextType.Resource, 84),
            BuildingStyle.Neoclassical => LanguageInfo.GetTextByType(TextType.Resource, 85),
            BuildingStyle.Scandinavian => LanguageInfo.GetTextByType(TextType.Resource, 86),
            BuildingStyle.Renaissance => LanguageInfo.GetTextByType(TextType.Resource, 87),
            _ => throw new System.NotImplementedException($"langauge for {bs}"),
            //8 = european
        };
        #endregion methods
    }
}