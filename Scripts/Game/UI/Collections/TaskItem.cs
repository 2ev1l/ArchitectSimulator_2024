using Game.Serialization.World;
using Game.UI.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Collections
{
    internal class TaskItem : MonoBehaviour, IListUpdater<PlayerTaskData>
    {
        #region fields & properties
        private PlayerTaskData value = null;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private CustomCheckbox checkbox;
        [SerializeField] private PopupLayoutElement popup;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(PlayerTaskData param)
        {
            value = param;
            text.text = value.Info.NameInfo.Text;
            checkbox.CurrentState = false;
            popup.HideImmediately();
            popup.Show();
        }
        #endregion methods
    }
}