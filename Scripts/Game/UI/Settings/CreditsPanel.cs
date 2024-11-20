using Game.UI.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Collections.Generic;

namespace Game.UI.Settings
{
    public class CreditsPanel : MonoBehaviour
    {
        #region fields & properties
        [SerializeField][TextArea(0, 200)] private string nameList;
        [SerializeField] private InfinityItemList<StringItem, string> stringItemList = new();
        private bool IsLoaded = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            if (!IsLoaded)
            {
                LoadNames();
            }
        }
        private void LoadNames()
        {
            IsLoaded = true;
            string[] names = nameList.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            stringItemList.UpdateListDefault(names, x => x);
        }
        #endregion methods
    }
}