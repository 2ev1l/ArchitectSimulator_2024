using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class BlueprintPolygonUnitInfo
    {
        #region fields & properties
        public Vector3 LocalPosition => localPosition;
        [SerializeField] private Vector3 localPosition;
        public IReadOnlyList<Vector2> TexturePoints => texturePoints;
        [SerializeField] private List<Vector2> texturePoints = new();
        public BuildingFloor FloorPlaced => floorPlaced;
        [SerializeField] private BuildingFloor floorPlaced;
        #endregion fields & properties

        #region methods
        public BlueprintPolygonUnitInfo(Vector3 localPosition, List<Vector2> texturePoints, BuildingFloor floorPlaced)
        {
            this.localPosition = localPosition;
            this.texturePoints = texturePoints;
            this.floorPlaced = floorPlaced;
        }
        #endregion methods
    }
}