using System.Collections.Generic;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    public interface IReadOnlyResources<T> where T : ResourceData
    {
        /// <summary>
        /// Invokes only for new entries
        /// </summary>
        public UnityAction<T> OnResourceAdded { get; set; }
        /// <summary>
        /// Invokes only if resource is completely exhausted
        /// </summary>
        public UnityAction<T> OnResourceRemoved { get; set; }
        public IReadOnlyList<T> Items { get; }
    }
}