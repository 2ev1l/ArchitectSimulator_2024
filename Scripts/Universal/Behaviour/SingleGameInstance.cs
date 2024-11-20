using UnityEngine;

namespace Universal.Behaviour
{
    [ExecuteAlways]
    public class SingleGameInstance : MonoBehaviour
    {
        #region fields
        public static SingleGameInstance Instance => instance;
        private static SingleGameInstance instance;
        #endregion fields

        #region methods
        private void OnEnable()
        {
            if (instance == null)
                ForceInitialize();
        }
        public void ForceInitialize()
        {
            instance = this;
        }
        #endregion methods
    }
}