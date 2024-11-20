using DebugStuff;
using EditorCustom.Attributes;
using Game.DataBase;
using Game.Events;
using Game.Serialization.World;
using Game.UI.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Collections;
using Universal.Collections.Generic;
using Universal.Events;
using Universal.Time;

namespace Game.UI.Overlay
{
    public class ImportantInfoRequestExecutor : RequestExecutorBehaviour
    {
        #region fields & properties
        [SerializeField] private ObjectPool<DestroyablePoolableObject> textPool;
        private Queue<string> textQueue = new();
        private TimeDelay textDelay = new();
        #endregion fields & properties

        #region methods
        private void CheckQueue()
        {
            if (!textDelay.CanActivate) return;
            if (textQueue.Count == 0) return;
            string currentText = textQueue.Dequeue();
            if (!TryShowText(currentText, out ImportantInfoContent content)) return;
            textDelay.Delay = content.LiveTime - 2;
            textDelay.OnDelayReady = CheckQueue;
            textDelay.Activate();
        }
        private void EnqueueInfo(string text)
        {
            textQueue.Enqueue(text);
            CheckQueue();
        }
        private void EnqueueInfo(IEnumerable<string> infos)
        {
            foreach (string text in infos)
            {
                textQueue.Enqueue(text);
            }
            CheckQueue();
        }
        private bool TryShowText(string text, out ImportantInfoContent content)
        {
            content = (ImportantInfoContent)textPool.GetObject();
            content.UpdateUI(text);
            return true;
        }
        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is not ImportantInfoRequest infoRequest) return false;
            EnqueueInfo(infoRequest.Text);
            infoRequest.Close();
            return true;
        }
        #endregion methods

#if UNITY_EDITOR
        [Button(nameof(ShowInfo))]
        private void ShowInfo()
        {
            EnqueueInfo("Video provides a great opportunity to prove your point.");
        }
#endif //UNITY_EDITOR

    }
}