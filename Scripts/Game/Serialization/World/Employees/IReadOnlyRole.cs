using UnityEngine.Events;

namespace Game.Serialization.World
{
    public interface IReadOnlyRole
    {
        public UnityAction<ISingleEmployee> OnHired { get; set; }
        public UnityAction<ISingleEmployee> OnFired { get; set; }
        public ISingleEmployee Employee { get; }
        public bool IsEmployeeHired();
    }
}