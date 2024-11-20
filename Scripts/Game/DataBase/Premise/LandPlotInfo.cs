using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class LandPlotInfo : PremiseInfo, IBlueprintHandler
    {
        #region fields & properties
        public Sprite BuildedSprite => buildedSprite;
        [SerializeField] private Sprite buildedSprite;
        public BlueprintInfo BlueprintInfo => blueprintReference.Data;
        [SerializeField] private BlueprintInfoSO blueprintReference;
        public int TargetWindows => targetWindows;
        [SerializeField][Min(0)] private int targetWindows = 0;
        public int TargetRooms => targetRooms;
        [SerializeField][Min(0)] private int targetRooms = 0;
        public int TargetBuildingPrice => targetBuildingPrice;
        [SerializeField][Min(1)] private int targetBuildingPrice = 1;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}