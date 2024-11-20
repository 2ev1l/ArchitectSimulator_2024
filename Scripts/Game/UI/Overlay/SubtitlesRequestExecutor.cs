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
    public class SubtitlesRequestExecutor : RequestExecutorBehaviour
    {
        #region fields & properties
        [SerializeField] private ObjectPool<DestroyablePoolableObject> subtitlesPool;
        private Queue<int> subtitlesQueue = new();
        private TimeDelay subtitlesDelay = new();
        #endregion fields & properties

        #region methods
        private void CheckQueue()
        {
            if (!subtitlesDelay.CanActivate) return;
            if (subtitlesQueue.Count == 0) return;
            int currentId = subtitlesQueue.Dequeue();
            if (!TryShowSubtitle(currentId, out SubtitlesContent content)) return;
            subtitlesDelay.Delay = content.LiveTime - 2;
            subtitlesDelay.OnDelayReady = CheckQueue;
            subtitlesDelay.Activate();
        }
        private void EnqueueSubtitiles(IEnumerable<int> subtitlesId)
        {
            foreach (int subtitleId in subtitlesId)
            {
                subtitlesQueue.Enqueue(subtitleId);
            }
            CheckQueue();
        }
        private bool TryShowSubtitle(int id, out SubtitlesContent content)
        {
            content = null;
            if (!GameData.Data.PlayerData.SubtitlesData.TryAddPlayedSubtitle(id)) return false;
            string text = LanguageLoader.GetTextByType(TextType.Subtitle, id);
            content = (SubtitlesContent)subtitlesPool.GetObject();
            content.UpdateUI(text);
            return true;
        }
        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is not SubtitleRequest subtitleRequest) return false;
            EnqueueSubtitiles(subtitleRequest.Ids);
            subtitleRequest.Close();
            return true;
        }
        #endregion methods

#if UNITY_EDITOR
        [Button(nameof(ShowSubtitles))]
        private void ShowSubtitles()
        {
            GameData.Data.PlayerData.SubtitlesData.TryRemovePlayedSubtitle(0);
            GameData.Data.PlayerData.SubtitlesData.TryRemovePlayedSubtitle(1);
            EnqueueSubtitiles(new List<int>() { 0, 1 });
        }
#endif //UNITY_EDITOR

    }
}