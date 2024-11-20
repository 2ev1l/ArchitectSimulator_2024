using Game.DataBase;
using Game.UI.Overlay;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Universal.Behaviour;

namespace Tests.PlayMode
{
    [System.Serializable]
    public class AssetLoader
    {
        #region fields & properties
        private static readonly GameObject textDataPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Core/Text Data.prefab");
        private static readonly GameObject dbPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Core/DB.prefab");
        public static GameObject SingleGameInstance = null;
        public static GameObject Camera = null;
        public static GameObject Db = null;
        public static GameObject TextDataObj = null;
        #endregion fields & properties

        #region methods
        public static void InitDB()
        {
            if (!TryInstantiateSingle(dbPrefab, ref Db)) return;
            DB.Instance = Db.GetComponent<DB>();
        }
        public static void InitCamera()
        {
            if (!TryInstantiateSingle(CanvasInitializer.OverlayCameraName, ref Camera)) return;
            Camera.AddComponent<Camera>();
        }
        public static void InitSingleGameInstance()
        {
            if (!TryInstantiateSingle("SingleGameInstance", ref SingleGameInstance)) return;
            SingleGameInstance si = SingleGameInstance.AddComponent<SingleGameInstance>();
            si.ForceInitialize();
        }
        public static void InitTextData()
        {
            if (!TryInstantiateSingle(textDataPrefab, ref TextDataObj)) return;
            TextData textData = TextDataObj.GetComponent<TextData>();
            TextData.LoadedData = textData.EnglishLanguageData;
        }
        public static void InitInstances()
        {
            InitCamera();
            InitSingleGameInstance();
            InitDB();
            InitTextData();
        }

        private static bool TryInstantiateSingle(string objectName, ref GameObject reference)
        {
            if (IsInstantiated(reference)) return false;
            reference = new(objectName);
            return true;
        }
        private static bool TryInstantiateSingle(GameObject prefab, ref GameObject reference)
        {
            if (IsInstantiated(reference)) return false;
            reference = Instantiate(prefab);
            return true;
        }
        private static bool IsInstantiated(GameObject obj) => obj != null;
        private static GameObject Instantiate(GameObject obj) => Object.Instantiate(obj);
        #endregion methods
    }
}