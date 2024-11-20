using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Serialization.World;
using Game.DataBase;
using EditorCustom.Attributes;
using DebugStuff;
using UnityEngine.UIElements;

namespace Game.Environment
{
    public class BuildingCreator : MonoBehaviour
    {
        #region fields & properties
        public const float GROUND_OFFSET = 0.2f;
        public const float WORKFLOW_TO_WORLD_SCALE = 200;

        public Transform ParentForSpawn
        {
            get
            {
                if (parentForSpawn == null)
                    parentForSpawn = transform;
                return parentForSpawn;
            }
        }
        private Transform parentForSpawn;
        public IReadOnlyCollection<GameObject> SpawnedObjects => spawnedObjects;
        private readonly HashSet<GameObject> spawnedObjects = new();
        #endregion fields & properties

        #region methods
        private void DestroySpawnedObjects()
        {
            foreach (GameObject obj in spawnedObjects)
            {
                Destroy(obj);
            }
            spawnedObjects.Clear();
        }
        public void BuildNewConstruction(BlueprintBaseData baseData)
        {
            DestroySpawnedObjects();
            BuildingFloor currentFloor = BuildingFloor.F1_Flooring;
            BuildingFloor prevFloor = currentFloor;
            BuildingFloor maxFloor = baseData.BuildingData.MaxFloor;
            int currentF2Count = 0;
            int additionalF2Count = baseData.BuildingData.Floor2AdditionalCount;
            float currentOffsetY = GROUND_OFFSET;

            while (prevFloor != maxFloor)
            {
                SpawnResources(currentOffsetY, currentF2Count, currentFloor, baseData, out float increaseOffsetY);
                currentOffsetY += increaseOffsetY;
                if (currentFloor == BuildingFloor.F2 && currentF2Count < additionalF2Count)
                {
                    currentF2Count++;
                    prevFloor = BuildingFloor.F1;
                    currentFloor = BuildingFloor.F2_FlooringRoof;
                    continue;
                }
                prevFloor = currentFloor;
                currentFloor = currentFloor.GetNextFloor();
                if (currentFloor == 0) break;
            }
        }
        private void SpawnResources(float currentOffsetY, int currentF2Count, BuildingFloor currentFloor, BlueprintBaseData baseData, out float increaseOffsetY)
        {
            bool increaseInitialized = false;
            bool canSpawnStair = (currentFloor == BuildingFloor.F2 && currentF2Count < baseData.BuildingData.Floor2AdditionalCount) || currentFloor == BuildingFloor.F1;
            BuildingFloor stairsSpawnFloor = BuildingFloor.F1;
            if (currentF2Count % 2 == 0 && currentFloor == BuildingFloor.F2)
            {
                stairsSpawnFloor = BuildingFloor.F2;
            }
            increaseOffsetY = 0;
            foreach (BlueprintResourceData resource in baseData.BlueprintResources)
            {
                ConstructionResourceInfo resourceInfo = resource.ResourceInfo;
                BuildingFloor floorPlaced = resource.UnitData.FloorPlaced;
                ConstructionSubtype resourceSubtype = resourceInfo.ConstructionSubtype;
                if (resourceSubtype == ConstructionSubtype.Staircase && !canSpawnStair) continue;
                if (floorPlaced != currentFloor)
                {
                    if (resourceSubtype == ConstructionSubtype.Staircase)
                    {
                        if (floorPlaced != stairsSpawnFloor) continue;
                    }
                    else continue;
                }

                if (!increaseInitialized && resourceSubtype != ConstructionSubtype.Staircase)
                {
                    increaseOffsetY = resourceInfo.Prefab.SizeMeters.y;
                    increaseInitialized = true;
                }

                if (resourceSubtype == ConstructionSubtype.Staircase)
                {
                    if (floorPlaced != stairsSpawnFloor) continue;
                }
                SpawnResource(currentOffsetY, resource, resourceInfo);
            }
            if (currentFloor == BuildingFloor.F1_Flooring || currentFloor == BuildingFloor.F2_FlooringRoof || currentFloor == BuildingFloor.F3_Roof)
                increaseOffsetY = 0;
        }
        private void SpawnResource(float offsetY, BlueprintResourceData resource, ConstructionResourceInfo resourceInfo)
        {
            ResourcePrefab instantiated = Instantiate(resourceInfo.Prefab, ParentForSpawn);
            spawnedObjects.Add(instantiated.gameObject);
            Transform instantiatedTransform = instantiated.Transform;
            Vector3 workflowPosition = resource.UnitData.LocalPosition;
            instantiatedTransform.localPosition = new(workflowPosition.x / WORKFLOW_TO_WORLD_SCALE, offsetY, workflowPosition.y / WORKFLOW_TO_WORLD_SCALE);
            Vector3 ownEulerAngles = instantiatedTransform.localEulerAngles;
            int additionalAngleScale = resource.UnitData.Rotation % 2 == 0 ? 180 : 0;
            Vector3 newLocalEulerAngles = new(ownEulerAngles.x, ownEulerAngles.y + additionalAngleScale + (int)(90 * resource.UnitData.Rotation), ownEulerAngles.z);
            instantiatedTransform.localEulerAngles = newLocalEulerAngles;
            instantiated.ChangeColor(resource.ChoosedColorId);
        }
        #endregion methods

#if UNITY_EDITOR
        [Title("Debug")]
        [SerializeField] private bool doDebug = true;
        [SerializeField][DrawIf(nameof(doDebug), true)] private bool debugAlways = false;

        private void OnDrawGizmosSelected()
        {
            if (!doDebug) return;
            if (debugAlways) return;
            DebugDraw();
        }
        private void OnDrawGizmos()
        {
            if (!doDebug) return;
            if (!debugAlways) return;
            DebugDraw();
        }

        private void DebugDraw()
        {
            Gizmos.DrawWireCube(ParentForSpawn.position + Vector3.up * GROUND_OFFSET, new Vector3(40.96f, 0, 40.96f));
        }

#endif //UNITY_EDITOR
    }
}