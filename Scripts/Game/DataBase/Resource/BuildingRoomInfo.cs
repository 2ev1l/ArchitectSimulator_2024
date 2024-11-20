using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class BuildingRoomInfo
    {
        #region fields & properties
        public static readonly float MultipleRoomsAreaScale = 1.42f;
        public BuildingRoom Room => room;
        [SerializeField] private BuildingRoom room;
        public float TargetArea => targetArea;
        [SerializeField][Min(0.2f)] private float targetArea = 0.3f;
        #endregion fields & properties

        #region methods
        public string ToLanguage()
        {
            return $"   {room.ToLanguage()} x {targetArea:F2} m2";
        }
        #endregion methods
    }
}