using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Elements;
using Game.UI.Overlay.Computer.Collections;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class AvailableBlueprintItem : BlueprintInfoItem
    {
        #region fields & properties
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private CustomButton addButton;
        private string lastChangedName = "";
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            if (nameInputField != null)
                nameInputField.onDeselect.AddListener(ChangeName);
            addButton.OnClicked += TryAddBlueprint;
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            if (nameInputField != null)
                nameInputField.onDeselect.RemoveListener(ChangeName);
            addButton.OnClicked -= TryAddBlueprint;
        }
        protected virtual bool IsNewNameAllowed(string name)
        {
            return BlueprintData.IsNameAllowed(name);
        }
        protected void ChangeName(string name)
        {
            if (lastChangedName.Equals(name)) return;
            if (!IsNewNameAllowed(name))
            {
                nameInputField.text = lastChangedName;
                return;
            }
            nameInputField.text = name;
            lastChangedName = name;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            lastChangedName = $"B-{Context.Id:000}";
            nameInputField.text = lastChangedName;
        }

        private void TryAddBlueprint()
        {
            BlueprintData data = new(nameInputField.text, Context.Id, new List<BlueprintResourceData>());
            if (!GameData.Data.BlueprintsData.TryAddNewBlueprint(data, out string lockReason))
            {
                InfoRequest ir = new(null, LanguageLoader.GetTextByType(TextType.Game, 37), lockReason);
                ir.Send();
                return;
            }
            OnBlueprintAdded(data);
        }
        protected virtual void OnBlueprintAdded(BlueprintData added) { }
        #endregion methods
    }
}