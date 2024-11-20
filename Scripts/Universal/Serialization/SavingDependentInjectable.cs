using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Serialization
{
    [System.Serializable]
    public abstract class SavingDependentInjectable : Zenject.IInitializable, IDisposable
    {
        #region fields & properties
        private bool isSubscribedAtSaveReload = false;
        #endregion fields & properties

        #region methods
        public virtual void Awake()
        {

        }
        public virtual void Initialize()
        {
            SubscribeOnSaveReload();
        }
        public virtual void Dispose()
        {
            UnsubscribeOnSaveReload();
        }
        private void SubscribeOnSaveReload()
        {
            if (isSubscribedAtSaveReload) return;
            isSubscribedAtSaveReload = true;

            SavingUtils.OnDataReset += DisposeInitializeAwake;
            SavingUtils.OnSettingsReset += DisposeInitializeAwake;
        }
        private void UnsubscribeOnSaveReload()
        {
            if (!isSubscribedAtSaveReload) return;
            isSubscribedAtSaveReload = false;

            SavingUtils.OnDataReset -= DisposeInitializeAwake;
            SavingUtils.OnSettingsReset -= DisposeInitializeAwake;
        }
        private void DisposeInitializeAwake()
        {
            Dispose();
            Initialize();
            Awake();
        }
        #endregion methods
    }
}