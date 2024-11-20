using EditorCustom.Attributes;
using Game.DataBase;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment
{
    public class BuildingLoader : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private BuildingCreator creator;
        [SerializeField] private GameObject placeholderPrefab;
        [SerializeField] private int referenceId;
        private GameObject instantiatedPlaceholder = null;
        private ConstructionData data = null;
        private bool isConstructionBuild = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            LoadData();
            TryBuildConstruction();
            Subscribe();
        }
        private void OnDisable()
        {
            Unsubscribe();
        }
        private void Subscribe()
        {
            GameData.Data.ConstructionsData.OnConstructionAdded += TryLoadAddedConstruction;
            if (data == null) return;
            data.OnBuildCompleted += TryBuildConstruction;
        }
        private void Unsubscribe()
        {
            GameData.Data.ConstructionsData.OnConstructionAdded -= TryLoadAddedConstruction;
            if (data == null) return;
            data.OnBuildCompleted -= TryBuildConstruction;
        }
        private void TryLoadAddedConstruction(ConstructionData constructionData)
        {
            if (constructionData.BlueprintInfoId != referenceId) return;
            Unsubscribe();
            this.data = constructionData;
            Subscribe();
            TryBuildConstruction();
        }
        private void LoadData()
        {
            GameData.Data.ConstructionsData.TryGet(referenceId, out data);
        }
        [Todo("UI spawn placeholder")]
        private void TryBuildConstruction()
        {
            if (isConstructionBuild) return;
            if (data == null) return;
            if (!data.IsBuilded)
            {
                if (instantiatedPlaceholder == null)
                {
                    instantiatedPlaceholder = Instantiate(placeholderPrefab, creator.ParentForSpawn);
                    instantiatedPlaceholder.transform.localPosition = Vector3.zero;
                }
                return;
            }
            if (instantiatedPlaceholder != null)
                Destroy(instantiatedPlaceholder);
            creator.BuildNewConstruction(data);
            isConstructionBuild = true;
        }
        #endregion methods
    }
}