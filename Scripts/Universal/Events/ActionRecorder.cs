using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Universal.Events
{
    [System.Serializable]
    public class ActionRecorder
    {
        #region fields & properties
        private List<Action> actions = new();
        private int size = 0;
        #endregion fields & properties

        #region methods
        public void Clear()
        {
            actions.Clear();
        }
        /// <summary>
        /// Invokes last action and removes it
        /// </summary>
        public void InvokeLast()
        {
            int count = actions.Count;
            if (count == 0) return;
            actions[count - 1].Invoke();
            actions.RemoveAt(count - 1);
        }
        /// <summary>
        /// Removes first action if size was reached
        /// </summary>
        /// <param name="action"></param>
        public void ReplaceRecord(Action action)
        {
            int count = actions.Count;
            if (size > 0 && count >= size)
            {
                actions.RemoveAt(0);
            }
            Record(action);
        }
        public bool TryRecord(Action action)
        {
            int count = actions.Count;
            if (size > 0 && count >= size) return false;
            Record(action);
            return true;
        }
        private void Record(Action action)
        {
            actions.Add(action);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">0 means infinity</param>
        public ActionRecorder(int size)
        {
            size = Mathf.Max(size, 0);
            this.size = size;
        }
        #endregion methods
    }
}