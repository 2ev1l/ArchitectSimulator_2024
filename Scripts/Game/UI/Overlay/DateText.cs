using DebugStuff;
using EditorCustom.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Game.UI.Overlay
{
    public class DateText : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private int dateSize = 12;
        [SerializeField] private bool showHours = true;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            SetDate();
            if (showHours)
                InvokeRepeating(nameof(SetDate), 30, 30);
        }
        private void OnDisable()
        {
            CancelInvoke(nameof(SetDate));
        }
        private void SetDate()
        {
            DateTime currentTime = DateTime.Now.ToLocalTime();
            string monthTime = currentTime.ToString("d");
            if (showHours)
            {
                string hoursTime = currentTime.ToString("t");
                text.text = $"{hoursTime}\n<size={dateSize}>{monthTime}</size>";
            }
            else
            {
                text.text = $"<size={dateSize}>{monthTime}</size>";
            }
        }

#if UNITY_EDITOR
        [Title("Debug")]
        [SerializeField] private bool updateDate = true;
        private void OnValidate()
        {
            if (!updateDate) return;
            SetDate();
        }
#endif //UNITY_EDITOR
        #endregion methods
    }
}