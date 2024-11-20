using System.Collections.Generic;
using UnityEngine.Events;

namespace Game.Serialization.World
{
    public interface IReadOnlyDivision
    {
        public UnityAction<IEmployee> OnEmployeeHired { get; set; }
        public UnityAction<IEmployee> OnEmployeeFired { get; set; }

        public IReadOnlyList<IEmployee> Employees { get; }
    }
}