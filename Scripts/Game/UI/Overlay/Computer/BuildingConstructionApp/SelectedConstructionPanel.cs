using EditorCustom.Attributes;
using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class SelectedConstructionPanel : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private ConstructionDataItem constructionDataItem;
        [SerializeField] private TextMeshProUGUI selectedBuildersText;
        [SerializeField] private TextMeshProUGUI timeToCompleteText;
        [SerializeField] private GameObject nextStepButton;
        [SerializeField] private SelectableBuilderItemList selectableBuilderItemList;
        [System.NonSerialized] private ConstructionData constructionData = null;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            OnSelectedBuildersChanged();
            selectableBuilderItemList.OnSelectedBuildersChanged += OnSelectedBuildersChanged;
        }
        private void OnDisable()
        {
            selectableBuilderItemList.OnSelectedBuildersChanged -= OnSelectedBuildersChanged;
            OnSelectedBuildersChanged();
        }
        private void OnSelectedBuildersChanged()
        {
            if (constructionData == null) return;
            IReadOnlyList<BuilderData> selected = selectableBuilderItemList.SelectedBuilders;
            nextStepButton.SetActive(selected.Count > 0);
            if (selected == null || selected.Count == 0)
            {
                selectedBuildersText.text = "0x";
                timeToCompleteText.text = "???";
                return;
            }
            selectedBuildersText.text = $"{selected.Count}x";
            timeToCompleteText.text = $"{constructionData.GetCompletionTime(selected)} m.";
        }
        [SerializedMethod]
        public void SaveData()
        {
            foreach (var builder in selectableBuilderItemList.SelectedBuilders)
            {
                constructionData.TryAddBuilder(builder);
            }
            constructionData.TryStartBuild();
        }
        public void UpdateData(ConstructionData constructionData)
        {
            this.constructionData = constructionData;
            constructionDataItem.OnListUpdate(constructionData);
        }
        #endregion methods
    }
}