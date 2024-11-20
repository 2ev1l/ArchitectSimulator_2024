using EditorCustom.Attributes;
using Game.Events;
using Game.Serialization.Settings;
using Game.Serialization.Settings.Input;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;
using Universal.Core;

namespace Game.UI.Collections
{
    public class KeyCodesItemList : InfinityItemListBase<KeyCodeItem, KeyCodeInfo>
    {
        #region fields & properties
        [SerializeField] private KeyCatcher keyCatcher;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            keyCatcher.OnCatchStart += DisableAllButtons;
            keyCatcher.OnKeyReturns += EnableAllButtons;
            keyCatcher.OnKeyCatched += EnableAllButtons;
            IReadOnlyList<KeyCodeItem> items = base.ItemList.Items;
            foreach (var item in items)
            {
                CheckDuplicate(item, items);
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            keyCatcher.OnCatchStart -= DisableAllButtons;
            keyCatcher.OnKeyReturns -= EnableAllButtons;
            keyCatcher.OnKeyCatched -= EnableAllButtons;
        }
        [SerializedMethod]
        public void ResetKeys()
        {
            SettingsData.Data.KeyCodeSettings.ResetKeys();
            UpdateListData();
        }
        public override void UpdateListData()
        {
            ItemList.UpdateListDefault(InputController.AllKeys, x => x);
            foreach (KeyCodeItem el in ItemList.Items)
            {
                CheckDuplicate(el, ItemList.Items);
                el.Button.OnClicked = delegate
                {
                    keyCatcher.CatchKey(el.Value);
                    el.Text.text += $" [ESC]";
                };
            }
        }
        private void CheckDuplicate(KeyCodeItem item, IEnumerable<KeyCodeItem> list)
        {
            bool duplicate = list.Exists(x => x.Value.Key == item.Value.Key && x != item, out _);
            item.SetDuplicate(duplicate);
        }
        private void EnableAllButtons(KeyCodeInfo _) => EnableAllButtons();
        private void EnableAllButtons()
        {
            foreach (KeyCodeItem el in ItemList.Items)
            {
                CheckDuplicate(el, ItemList.Items);
                el.EnableButton();
            }
        }
        private void DisableAllButtons()
        {
            foreach (KeyCodeItem el in ItemList.Items)
            {
                el.DisableButton();
            }
        }
        #endregion methods
    }
}