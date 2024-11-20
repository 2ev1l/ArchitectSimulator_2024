using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    #region enum
    public enum ResourceColor
    {
        White,
        Gray,
        Black,
        Cyan,
        Red,
        Magenta,
        Yellow,
        Green
    }
    #endregion enum

    public static class ResourceColorExtension
    {
        #region methods
        /// <summary>
        /// Nice looking colors
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Color ToColorRGB(this ResourceColor color) => color switch
        {
            ResourceColor.White => Color.white,
            ResourceColor.Gray => new(0.7235f, 0.7235f, 0.7235f),
            ResourceColor.Black => new(0.4235f, 0.4235f, 0.4235f),
            ResourceColor.Cyan => new(0.6784f, 0.9529f, 0.9529f),
            ResourceColor.Red => new(0.9843f, 0.5137f, 0.5647f),
            ResourceColor.Magenta => new(0.9529f, 0.6784f, 0.8627f),
            ResourceColor.Yellow => new(1, 0.9725f, 0.6f),
            ResourceColor.Green => new(0.6f, 1, 0.6549f),
            _ => throw new System.NotImplementedException($"resourceColor to color: {color}")
        };
        #endregion methods
    }
}