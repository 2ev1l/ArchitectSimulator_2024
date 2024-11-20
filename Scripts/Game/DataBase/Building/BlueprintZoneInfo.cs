using Game.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.DataBase.PolygonBlueprintGraphic;

namespace Game.DataBase
{
    [System.Serializable]
    public class BlueprintZoneInfo : BlueprintPolygonUnitInfo
    {
        #region fields & properties
        public PlacementType Placement => placement;
        [SerializeField] private PlacementType placement;
        #endregion fields & properties

        #region methods
        public BlueprintZoneInfo(Vector3 localPosition, List<Vector2> texturePoints, BuildingFloor floorPlaced, PlacementType placement) : base(localPosition, texturePoints, floorPlaced)
        {
            this.placement = placement;
        }
        #endregion methods
    }
}