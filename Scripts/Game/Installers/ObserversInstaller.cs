using DebugStuff;
using EditorCustom.Attributes;
using Game.Environment;
using Game.Environment.Observers;
using Game.Events;
using Game.Serialization.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Behaviour;
using Zenject;

namespace Game.Installers
{
    public class ObserversInstaller : MonoInstaller
    {
        #region fields & properties
        [SerializeField] private PlayerTasksObserver tasksObserver;
        [SerializeField] private StatsObserver statsObserver;
        [SerializeField] private LocationObserver locationObserver;
        [SerializeField] private MonthObserver monthObserver;
        [SerializeField] private AchievementsObserver achievementsObserver;
        [SerializeField] private MusicObserver musicObserver;
        #endregion fields & properties

        #region methods
        public override void InstallBindings()
        {
            BindObserverFromInstance(tasksObserver);
            BindObserverFromInstance(statsObserver);
            BindObserverFromInstance(locationObserver);
            BindObserverFromInstance(monthObserver);
            BindObserverFromInstance(achievementsObserver);
            BindObserverFromInstance(musicObserver);
        }
        private void BindObserverFromInstance<T>(T instance) where T : Observer
        {
            Container.BindInterfacesAndSelfTo<T>().FromInstance(instance).AsSingle();
            Container.QueueForInject(locationObserver);
        }
        #endregion methods

#if UNITY_EDITOR
        [Title("Tests")]
        [SerializeField][DontDraw] private bool ___testBool;
        [SerializeField][Label("Achievement To Set")] private string ___testAchievement;

        [Button(nameof(TestMonthObserverNextMonth))]
        private void TestMonthObserverNextMonth()
        {
            monthObserver.OnMonthChanged(GameData.Data.PlayerData.MonthData.CurrentMonth + 1);
        }
        [Button(nameof(TestMonthObserverNextMonthReal))]
        private void TestMonthObserverNextMonthReal()
        {
            GameData.Data.PlayerData.MonthData.StartNextMonth();
        }
        [Button(nameof(TestShowMoneyPopup))]
        private void TestShowMoneyPopup()
        {
            statsObserver.SendMoneyPopupRequest(1000, -1000);
            statsObserver.SendMoneyPopupRequest(1000, 15);
            //check ui with many digits
            statsObserver.SendMoneyPopupRequest(100000, -1000);
            statsObserver.SendMoneyPopupRequest(100000, -10000);
            statsObserver.SendMoneyPopupRequest(100000, -100000);
            statsObserver.SendMoneyPopupRequest(1000000, 100000);
            statsObserver.SendMoneyPopupRequest(10000000, 100000);
            statsObserver.SendMoneyPopupRequest(10000000, 1000000);
            statsObserver.SendMoneyPopupRequest(100000000, 1000000);
        }
        [Button(nameof(TestShowMoneyPopupReal))]
        private void TestShowMoneyPopupReal()
        {
            GameData.Data.PlayerData.Wallet.TryIncreaseValue(20);
            GameData.Data.PlayerData.Wallet.TryDecreaseValue(15);
        }
        [Button(nameof(TestShowMoodPopup))]
        private void TestShowMoodPopup()
        {
            statsObserver.SendMoodPopupRequest(100, -10);
            statsObserver.SendMoodPopupRequest(50, 15);
        }
        [Button(nameof(TestShowRatingPopup))]
        private void TestShowRatingPopup()
        {
            statsObserver.SendRatingPopupRequest(20, -10);
            statsObserver.SendRatingPopupRequest(25, 15);
        }
        [Button(nameof(TestIncreaseRating))]
        private void TestIncreaseRating()
        {
            GameData.Data.CompanyData.Rating.TryIncreaseValue(1);
        }
        [Button(nameof(TestShowTimePopup))]
        private void TestShowTimePopup()
        {
            statsObserver.SendTimePopupRequest(70, -10);
            statsObserver.SendTimePopupRequest(100, 15);
        }
        [Button(nameof(TestShowTaskStartPopup))]
        private void TestShowTaskStartPopup()
        {
            statsObserver.SendTaskStartPopupRequest();
        }
        [Button(nameof(TestShowTaskEndPopup))]
        private void TestShowTaskEndPopup()
        {
            statsObserver.SendTaskCompletedPopupRequest();
        }
        [Button(nameof(TestShowNewLandPlotOfferPopup))]
        private void TestShowNewLandPlotOfferPopup()
        {
            statsObserver.SendNewLandPlotOfferPopupRequest();
        }
        [Button(nameof(TestSetAchievement))]
        private void TestSetAchievement()
        {
            if (!DebugCommands.IsApplicationPlaying()) return;
            AchievementsObserver.SetAchievement(___testAchievement);
        }

#endif //UNITY_EDITOR
    }
}