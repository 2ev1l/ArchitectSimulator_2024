using System.Collections.Generic;
using UnityEngine;
using Universal.Core;
using Zenject;

namespace Universal.Events
{
    /// <summary>
    /// Using HashSet instead of Action/Message etc. for better performance
    /// </summary>
    [System.Serializable]
    public class MessageController : ITickable, IFixedTickable, ILateTickable
    {
        #region fields & properties
        private static readonly HashSet<IUpdateSender> updateSenders = new();
        private static readonly HashSet<IFixedUpdateSender> fixedUpdateSenders = new();
        private static readonly HashSet<ILateUpdateSender> lateUpdateSenders = new();
        #endregion fields & properties

        #region methods
        private static void AddObjectByType(IMessageSender obj)
        {
            //don't want to do different (same) implementation in each class
            if (obj is IUpdateSender us) updateSenders.Add(us);
            if (obj is IFixedUpdateSender fs) fixedUpdateSenders.Add(fs);
            if (obj is ILateUpdateSender ls) lateUpdateSenders.Add(ls);
        }
        private static void RemoveObjectByType(IMessageSender obj)
        {
            if (obj is IUpdateSender us) updateSenders.Remove(us);
            if (obj is IFixedUpdateSender fs) fixedUpdateSenders.Remove(fs);
            if (obj is ILateUpdateSender ls) lateUpdateSenders.Remove(ls);
        }
        /// <summary>
        /// Don't need to call it more than one time for single object for initializing different interfaces
        /// </summary>
        /// <param name="obj"></param>
        public static void AddSender(IMessageSender obj) => AddObjectByType(obj);
        /// <summary>
        /// Don't need to call it more than one time for single object for initializing different interfaces
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveSender(IMessageSender obj) => RemoveObjectByType(obj);
        private void Update()
        {
            foreach (var el in updateSenders)
            {
                el.UpdateMessage();
            }
        }
        private void FixedUpdate()
        {
            foreach (var el in fixedUpdateSenders)
            {
                el.FixedUpdateMessage();
            }
        }
        private void LateUpdate()
        {
            foreach (var el in lateUpdateSenders)
            {
                el.LateUpdateMessage();
            }
        }

        public void Tick() => Update();
        public void FixedTick() => FixedUpdate();
        public void LateTick() => LateUpdate();
        #endregion methods
    }
}