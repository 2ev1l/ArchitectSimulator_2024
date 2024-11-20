using Game.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;
using Universal.Events;
using Zenject;

namespace Game.Installers
{
    public class RequestExecutorsInstaller : MonoInstaller
    {
        #region fields & properties
        [SerializeField] private RewardRequestExecutor rewardRequestExecutor;
        #endregion fields & properties

        #region methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RewardRequestExecutor>().FromInstance(rewardRequestExecutor).AsSingle().NonLazy();
        }
        #endregion methods
    }
}