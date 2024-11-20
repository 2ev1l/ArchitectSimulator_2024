using Game.Serialization.World;
using Game.UI.Collections;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class BlueprintDataItem : ContextActionsItem<BlueprintData>
    {
        #region fields & properties
        [SerializeField] private BlueprintInfoItem blueprintInfoItem;
        [SerializeField] private TMP_InputField nameInputField;
        #endregion fields & properties

        #region methods
        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            if (nameInputField != null)
                nameInputField.onDeselect.AddListener(ChangeName);
        }
        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            if (nameInputField != null)
                nameInputField.onDeselect.RemoveListener(ChangeName);
        }
        protected virtual bool IsNewNameAllowed(string name)
        {
            return BlueprintData.IsNameAllowed(name);
        }
        protected void ChangeName(string name)
        {
            if (Context.Name.Equals(name)) return;
            if (!IsNewNameAllowed(name))
            {
                nameInputField.text = Context.Name;
                return;
            }
            Context.TryChangeName(name);
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            nameInputField.text = Context.Name;
        }
        public override void OnListUpdate(BlueprintData param)
        {
            base.OnListUpdate(param);
            blueprintInfoItem.OnListUpdate(param.BlueprintInfo);
        }
        #endregion methods
    }
}