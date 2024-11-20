using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Environment
{
    public class SceneStructureDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        #region fields & properties
        [Title("Cameras")]
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private GameObject virtualCamera;

        [Title("Lighting")]
        [SerializeField] private GameObject globalVolume;
        [SerializeField] private GameObject skyBox;
        [SerializeField] private GameObject heightFog;

        [Title("Environment")]
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject startMap;
        [SerializeField] private GameObject cityMap;
        [SerializeField] private GameObject landPlotsMap;
        [SerializeField] private GameObject buildingsPreviewMap;
        [SerializeField] private GameObject buildingsPreviewBlueprintMaker;
        [SerializeField] private GameObject testMap;
        [SerializeField] private GameObject debugContext;

        [Title("UI")]
        [SerializeField] private GameObject defaultCanvas;
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private GameObject infoCanvas;

        [SerializeField] private GameObject laptopCanvas;
        [SerializeField] private GameObject laptopDesignAppFilters;
        [SerializeField] private List<GameObject> laptopDesignAppPanels;
        [SerializeField] private List<GameObject> laptopApps;

        [SerializeField] private GameObject constantCanvas;
        [SerializeField] private GameObject constantMonth;
        [SerializeField] private GameObject constantSubtitles;
        [SerializeField] private GameObject constantMessage;
        #endregion fields & properties

        #region methods
        private static void MustBeDisabledAndAppliedToPrefab(GameObject obj)
        {
            if (obj.activeSelf) LogError($"{obj.name} must be disabled and applied to prefab!", obj);
        }
        private static void MustBeDisabled(GameObject obj)
        {
            if (obj.activeSelf) LogError($"{obj.name} must be disabled", obj);
        }
        private static void MustBeEnabled(GameObject obj)
        {
            if (!obj.activeSelf) LogError($"{obj.name} must be enabled", obj);
        }
        private static void SetActive(GameObject obj, bool state)
        {
            obj.SetActive(state);
        }
        private static void LogError(string msg, Object context)
        {
            Debug.LogError(msg, context);
        }

        [Button(nameof(CheckRelease))]
        private void CheckRelease()
        {
            MustBeEnabled(mainCamera);
            MustBeEnabled(virtualCamera);

            MustBeEnabled(globalVolume);
            MustBeEnabled(skyBox);
            MustBeEnabled(heightFog);

            MustBeEnabled(player);
            MustBeEnabled(startMap);
            MustBeDisabled(cityMap);
            MustBeDisabled(landPlotsMap);
            MustBeDisabled(buildingsPreviewMap);
            MustBeDisabled(buildingsPreviewBlueprintMaker);
            MustBeDisabled(testMap);

            MustBeEnabled(defaultCanvas);
            MustBeDisabled(menuCanvas);
            MustBeDisabled(infoCanvas);
            MustBeDisabledAndAppliedToPrefab(debugContext);

            MustBeDisabled(laptopCanvas);
            MustBeDisabled(laptopDesignAppFilters);
            laptopApps.ForEach(x => MustBeDisabled(x));
            laptopDesignAppPanels.ForEach(x => MustBeDisabled(x));

            MustBeEnabled(constantCanvas);
            MustBeDisabled(constantMonth);
            MustBeEnabled(constantSubtitles);
            MustBeEnabled(constantMessage);

            Debug.Log("If you see no errors that mean game is ready for release.");
        }
        private void DebugLaptop()
        {
            Object[] objects = { virtualCamera, mainCamera, defaultCanvas, menuCanvas, infoCanvas, laptopCanvas };
            Undo.RecordObjects(objects, "Set to debug state");
            SetActive(virtualCamera, false);
            SetActive(mainCamera, true);
            mainCamera.transform.localEulerAngles = Vector3.zero;

            SetActive(defaultCanvas, false);
            SetActive(menuCanvas, false);
            SetActive(infoCanvas, false);
            SetActive(laptopCanvas, true);
        }

        [Button(nameof(DebugLaptopDesignApp))]
        private void DebugLaptopDesignApp()
        {
            DebugLaptop();
            Object[] objects = laptopApps.ToArray();
            Undo.RecordObjects(objects, "Set to debug state");
            SetActive(laptopApps[0], false);
            SetActive(laptopApps[1], false);
            SetActive(laptopApps[2], true);
            Selection.objects = new[] { laptopDesignAppPanels[1] };
        }
        [Button(nameof(OptimizePerformance))]
        private void OptimizePerformance()
        {
            SetActive(player, false);
            SetActive(startMap, false);
            SetActive(cityMap, false);
            SetActive(landPlotsMap, false);
        }
        #endregion methods

#endif //UNITY_EDITOR
    }
}