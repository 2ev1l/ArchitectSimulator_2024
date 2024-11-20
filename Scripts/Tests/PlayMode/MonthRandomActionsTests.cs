using Game.Serialization.World;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests.PlayMode
{
    [System.Serializable]
    public class MonthRandomActionsTests
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        [Test]
        public void TestExceptions()
        {
            InitTests();
            MonthRandomActions randomActions = new();
            OfficeData office = GameData.Data.CompanyData.OfficeData;
            for (int i = 0; i < 10000; ++i)
            {
                randomActions.TryGetAction(out _);
            }
            office.TryReplaceInfo(1);
            office.TryHireBuilder(new(0));
            office.TryHireBuilder(new(1));
            GameData.Data.PlayerData.HouseData.TryReplaceInfo(1);
            for (int i = 0; i < 100; ++i)
            {
                GameData.Data.CompanyData.ConstructionTasks.TryStartTask(i);
                GameData.Data.CompanyData.ConstructionTasks.TryCompleteTask(i);
            }
            GameData.Data.PlayerData.Tasks.TryStartTask(35);
            GameData.Data.PlayerData.Tasks.TryCompleteTask(35);

            for (int i = 0; i < 10000; ++i)
            {
                randomActions.TryGetAction(out _);
            }
        }
        [Test]
        public void TestFireBuilder()
        {
            InitTests();
            MonthRandomActions randomActions = new();
            OfficeData office = GameData.Data.CompanyData.OfficeData;
            office.TryReplaceInfo(1);
            office.TryHireBuilder(new(0));
            office.TryHireBuilder(new(1));
            ((BuilderData)office.Divisions.Builders.Employees[0]).SetBusy();
            for (int i = 0; i < 1000; ++i)
            {
                randomActions.TryGetEmployeesRandomAction(out _);
            }
            Assert.AreEqual(1, office.Divisions.Builders.Employees.Count);
            Assert.IsTrue(((BuilderData)office.Divisions.Builders.Employees[0]).IsBusy);
        }

        private void InitTests()
        {
            AssetLoader.InitInstances();
            GameData.SetData(new());
            for (int i = 0; i < 25; ++i)
            {
                GameData.Data.PlayerData.MonthData.StartNextMonth();
            }
        }
        #endregion methods
    }
}