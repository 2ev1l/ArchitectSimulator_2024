using Game.Serialization.World;
using Game.UI.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal.Events;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class SelectableBuilderItemList : SelectableItemList<BuilderData>, IRequestExecutor
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> - selected builders;
        /// </summary>
        public UnityAction OnSelectedBuildersChanged;
        public IReadOnlyList<BuilderData> SelectedBuilders => selectedBuilders;
        private List<BuilderData> selectedBuilders = new();
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            IReadOnlyDivision builders = GameData.Data.CompanyData.OfficeData.Divisions.Builders;
            builders.OnEmployeeHired += UpdateListData;
            builders.OnEmployeeFired += UpdateListData;
            RequestController.EnableExecution(this);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            IReadOnlyDivision builders = GameData.Data.CompanyData.OfficeData.Divisions.Builders;
            builders.OnEmployeeHired -= UpdateListData;
            builders.OnEmployeeFired -= UpdateListData;
            DeselectAll();
            RequestController.DisableExecution(this);
        }
        private void DeselectAll()
        {
            RequestController.DisableExecution(this);
            foreach (var builder in base.ItemList.Items)
            {
                builder.Deselect();
            }
            selectedBuilders.Clear();
            RequestController.EnableExecution(this);
            OnSelectedBuildersChanged?.Invoke();
        }
        private void RemoveSelectedBuilder(BuilderData builder)
        {
            selectedBuilders.Remove(builder);
            OnSelectedBuildersChanged?.Invoke();
        }
        private void AddSelectedBuilder(BuilderData builder)
        {
            selectedBuilders.Add(builder);
            OnSelectedBuildersChanged.Invoke();
        }
        public bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is SelectRequest<BuilderData> sr)
            {
                AddSelectedBuilder(sr.Selected);
                return true;
            }
            if (request is DeselectRequest<BuilderData> dr)
            {
                RemoveSelectedBuilder(dr.Deselected);
                return true;
            }
            return false;
        }
        private void UpdateListData(IEmployee _) => UpdateListData();
        protected override void UpdateCurrentItems(List<BuilderData> currentItemsReference)
        {
            currentItemsReference.Clear();
            IReadOnlyList<IEmployee> builders = GameData.Data.CompanyData.OfficeData.Divisions.Builders.Employees;
            int count = builders.Count;
            for (int i = 0; i < count; ++i)
            {
                BuilderData builder = (BuilderData)builders[i];
                if (builder.IsBusy) continue;
                currentItemsReference.Add(builder);
            }
        }
        public override void UpdateListData()
        {
            base.UpdateListData();
            DeselectAll();
        }
        #endregion methods
    }
}