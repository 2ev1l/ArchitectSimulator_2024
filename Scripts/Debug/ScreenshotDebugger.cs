using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorCustom.Attributes;
using UnityEngine.UIElements;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DebugStuff
{
    internal class ScreenshotDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        #region fields & properties
        [SerializeField] private GameObject firstCamera;
        [SerializeField] private GameObject secondCamera;
        [SerializeField] private Transform objectsParent;
        [SerializeField] private bool fixCameraOnEachScreenshot = true;
        private bool isCameraSwitched = false;
        #endregion fields & properties

        #region methods
        [Button(nameof(SwitchCamera))]
        private void SwitchCamera()
        {
            bool camSwitch = !firstCamera.activeSelf;
            firstCamera.SetActive(camSwitch);
            secondCamera.SetActive(!camSwitch);
        }
        [Button(nameof(FixHorizontalCameras))]
        private void FixHorizontalCameras()
        {
            Transform obj = GetActiveChild();
            if (!obj.GetChild(0).TryGetComponent(out Renderer render)) return;
            Vector3 extents = render.bounds.extents;
            float scale = 20;
            Vector3Int sizeCentimeters = new Vector3Int(Mathf.FloorToInt(extents.x * scale) * 10, Mathf.FloorToInt(extents.y * scale) * 10, Mathf.FloorToInt(extents.z * scale) * 10);
            Vector3 sizeMeters = Vector3.right * (sizeCentimeters.x / 100f) + Vector3.up * (sizeCentimeters.y / 100f) + Vector3.forward * (sizeCentimeters.z / 100f);
            Vector3 fcp = firstCamera.transform.position;
            Vector3 scp = secondCamera.transform.position;
            float camZ = Mathf.Sqrt(sizeMeters.x * fcp.y * 2) + (fcp.y / Mathf.Sqrt(sizeMeters.x));
            camZ -= camZ % 0.5f;
            firstCamera.transform.position = new(fcp.x, fcp.y, -camZ);
            secondCamera.transform.position = new(scp.x, scp.y, camZ);
        }
        [Button(nameof(IncreaseCameras))]
        private void IncreaseCameras()
        {
            Vector3 fcp = firstCamera.transform.position;
            Vector3 scp = secondCamera.transform.position;
            float step = 0.5f;
            firstCamera.transform.position = new(fcp.x, fcp.y, fcp.z - step);
            secondCamera.transform.position = new(scp.x, scp.y, scp.z + step);
        }
        [Button(nameof(DecreaseCameras))]
        private void DecreaseCameras()
        {
            Vector3 fcp = firstCamera.transform.position;
            Vector3 scp = secondCamera.transform.position;
            float step = -0.5f;
            firstCamera.transform.position = new(fcp.x, fcp.y, fcp.z - step);
            secondCamera.transform.position = new(scp.x, scp.y, scp.z + step);
        }
        [Button(nameof(SetNextChild))]
        private void SetNextChild()
        {
            int childs = objectsParent.childCount;
            bool activateNextChild = false;
            for (int i = 0; i < childs; ++i)
            {
                Transform child = objectsParent.GetChild(i);
                if (activateNextChild)
                {
                    child.gameObject.SetActive(true);
                    break;
                }
                if (child.gameObject.activeSelf)
                {
                    activateNextChild = true;
                    child.gameObject.SetActive(false);
                }
            }
            if (!activateNextChild)
                objectsParent.GetChild(0).gameObject.SetActive(true);
        }
        private Transform GetActiveChild()
        {
            int childs = objectsParent.childCount;
            for (int i = 0; i < childs; ++i)
            {
                Transform child = objectsParent.GetChild(i);
                if (child.gameObject.activeSelf)
                    return child;
            }
            return null;
        }
        [Button(nameof(GetNextScreenshot))]
        private void GetNextScreenshot()
        {
            if (isCameraSwitched)
            {
                SetNextChild();
            }
            SwitchCamera();
            isCameraSwitched = !isCameraSwitched;
            if (fixCameraOnEachScreenshot)
                FixHorizontalCameras();
        }
        #endregion methods
#endif //UNITY_EDITOR
    }
}