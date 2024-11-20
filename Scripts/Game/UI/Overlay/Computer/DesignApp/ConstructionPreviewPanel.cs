using Game.DataBase;
using Game.Environment;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Events;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class ConstructionPreviewPanel : MonoBehaviour, IRequestExecutor
    {
        #region fields & properties
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject map;
        [SerializeField] private BuildingCreator creator;
        [SerializeField] private Camera creatorCamera;
        [SerializeField] private bool subscribeAtRequests = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            if (subscribeAtRequests)
                RequestController.EnableExecution(this);
        }
        private void OnDisable()
        {
            DisableMap();
            if (subscribeAtRequests)
                RequestController.DisableExecution(this);
        }
        public bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is not ConstructionPreviewRequest previewRequest) return false;
            EnablePanel(previewRequest.BlueprintBaseData);
            return true;
        }
        public void EnablePanel(BlueprintBaseData data)
        {
            creator.BuildNewConstruction(data);
            Transform cameraTransform = creatorCamera.transform;
            Vector3 avgCoordsSum = Vector3.zero;
            foreach (GameObject spawned in creator.SpawnedObjects)
            {
                avgCoordsSum += spawned.transform.position;
            }
            avgCoordsSum /= creator.SpawnedObjects.Count;
            float maxDistanceToCoords = 0;
            float maxDistanceToCoordsY = 0;
            foreach (GameObject spawned in creator.SpawnedObjects)
            {
                Vector3 spawnedPos = spawned.transform.position;
                float newDistance = Vector3.Distance(spawnedPos, avgCoordsSum);
                float newDistanceY = Mathf.Abs(spawnedPos.y - avgCoordsSum.y);
                if (newDistance > maxDistanceToCoords)
                {
                    maxDistanceToCoords = newDistance;
                }
                if (newDistanceY > maxDistanceToCoordsY)
                {
                    maxDistanceToCoordsY = newDistanceY;
                }
            }
            cameraTransform.parent.position = avgCoordsSum;
            Vector3 cameraLocalPosition = Vector3.zero;
            Vector2 coordIncrease = data.BuildingData.BuildingType.GetCameraPreviewIncrease(data.BuildingData.BuildingStyle);

            cameraLocalPosition.x = -(maxDistanceToCoords + coordIncrease.x);
            cameraLocalPosition.z = -(maxDistanceToCoords + coordIncrease.x);

            cameraLocalPosition.y = Mathf.Pow(maxDistanceToCoordsY + coordIncrease.y + avgCoordsSum.y, 0.87f);
            cameraTransform.localPosition = cameraLocalPosition;
            creatorCamera.enabled = true;
            panel.SetActive(true);
            map.SetActive(true);
        }
        public void DisablePanel()
        {
            if (panel != null)
                panel.SetActive(false);
            DisableMap();
        }
        private void DisableMap()
        {
            if (creatorCamera != null)
                creatorCamera.enabled = false;
            if (map != null)
                map.SetActive(false);
        }
        #endregion methods
    }
}