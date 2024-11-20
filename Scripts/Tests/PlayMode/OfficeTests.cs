using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.DataBase;
using Game.Serialization.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Events;

namespace Tests.PlayMode
{
    public partial class WorldDataTests
    {
        public class OfficeTests
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            [Test]
            public void DesignEngineerTimeChangeTest()
            {
                PrepareAnyTest(out OfficeData office);
                office.TryReplaceInfo(0);
                RangedValue freeTime = GameData.Data.PlayerData.MonthData.FreeTime;
                Assert.AreEqual(freeTime.Value, MonthData.BaseFreeTime);
                Assert.IsTrue(office.TryHireDesignEngineer(new(0)));
                Assert.IsTrue(freeTime.Value > MonthData.BaseFreeTime);
                freeTime.TryDecreaseValue(10);
                int oldFreeTime = freeTime.Value;
                office.FireDesignEngineer();
                Assert.IsTrue(freeTime.Value < MonthData.BaseFreeTime);
                Assert.IsTrue(freeTime.Value < oldFreeTime);
            }

            [Test]
            public void AddRemoveDesignEngineerPositiveTest() => AddRemoveSingleEmployeePositiveTest(x => x.TryHireDesignEngineer(new(0)), x => x.FireDesignEngineer(), x => x.DesignEngineer);
            [Test]
            public void AddRemoveDesignEngineerNegativeTest() => AddRemoveSingleEmployeeNegativeTest(x => x.TryHireDesignEngineer(new(0)), x => x.FireDesignEngineer(), x => x.DesignEngineer);

            [Test]
            public void AddRemovePRPositiveTest() => AddRemoveSingleEmployeePositiveTest(x => x.TryHirePRManager(new(0)), x => x.FirePRManager(), x => x.PRManager);
            [Test]
            public void AddRemovePRNegativeTest() => AddRemoveSingleEmployeeNegativeTest(x => x.TryHirePRManager(new(0)), x => x.FirePRManager(), x => x.PRManager);

            [Test]
            public void AddRemoveHRPositiveTest() => AddRemoveSingleEmployeePositiveTest(x => x.TryHireHRManager(new(0)), x => x.FireHRManager(), x => x.HRManager);
            [Test]
            public void AddRemoveHRNegativeTest() => AddRemoveSingleEmployeeNegativeTest(x => x.TryHireHRManager(new(0)), x => x.FireHRManager(), x => x.HRManager);

            [Test]
            public void BuilderBusyStatusChangeTest()
            {
                PrepareAnyTest(out OfficeData office);
                ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
                ConstructionsData constructions = GameData.Data.ConstructionsData;
                constructions.TryAdd(new BlueprintData("1", 0, new List<BlueprintResourceData>()), new());
                office.TryReplaceInfo(0);
                office.TryHireBuilder(new(10));
                IReadOnlyDivision builders = office.Divisions.Builders;
                BuilderData builder = ((BuilderData)builders.Employees[0]);
                Assert.IsFalse(builder.IsBusy);
                Assert.IsTrue(constructions.Constructions[0].TryAddBuilder(builder));
                Assert.IsTrue(builder.IsBusy);
                constructions.Constructions[0].RemoveBuilders();
                Assert.IsFalse(builder.IsBusy);
                Assert.IsTrue(constructions.Constructions[0].TryAddBuilder(builder));
                Assert.IsTrue(builder.IsBusy);
                tasks.TryStartTask(0);
                tasks.TryAcceptTask(0, 0);
                tasks.TrySetTaskExpired(0);
                Assert.IsFalse(builder.IsBusy);
                Assert.IsFalse(((BuilderData)builders.Employees[0]).IsBusy);
            }
            [Test]
            public void AddRemoveBuilderPositiveTest() => AddRemoveEmployeePositiveTest<BuilderData>(x => x.TryHireBuilder(new(0)), (x, emp) => x.FireBuilder(emp), x => x.Builders);
            [Test]
            public void AddRemoveBuilderNegativeTest() => AddRemoveEmployeeNegativeTest<BuilderData>(x => x.TryHireBuilder(new(0)), (x, emp) => x.FireBuilder(emp), x => x.Builders);

            [Test]
            public void ReplaceOfficeWithEmployeesPositiveTest()
            {
                PrepareAnyTest(out OfficeData office);
                office.TryReplaceInfo(0);
                office.TryHireBuilder(new(10));
                Assert.IsTrue(office.TryReplaceInfo(1));
                BuilderData builder = (BuilderData)office.Divisions.Builders.Employees[0];
                Assert.AreEqual(1, office.Divisions.Builders.Employees.Count);
                Assert.AreSame(builder, office.Divisions.Builders.Employees[0]);
                Assert.AreEqual(10, builder.SkillLevel);
                Assert.IsTrue(office.TryReplaceInfo(0));
            }
            [Test]
            public void ReplaceOfficeWithEmployeesNegativeTest()
            {
                PrepareAnyTest(out OfficeData office);
                office.TryReplaceInfo(1);
                int maxEmployees = ((OfficeInfo)office.Info).MaximumEmployees;
                for (int i = 0; i < maxEmployees; ++i)
                {
                    office.TryHireBuilder(new(10));
                }
                Assert.AreEqual(maxEmployees, office.Divisions.Builders.Employees.Count);
                BuilderData builder = (BuilderData)office.Divisions.Builders.Employees[0];
                OfficeInfo newInfo = DB.Instance.OfficeInfo.Find(x => x.Data.MaximumEmployees < maxEmployees && x.Data.Id != office.Info.Id).Data;
                if (newInfo == null)
                {
                    Debug.LogError("Can't find office with less employees");
                    throw new System.InvalidProgramException();
                }
                Assert.IsFalse(office.TryReplaceInfo(newInfo.Id));
                Assert.AreEqual(maxEmployees, office.Divisions.Builders.Employees.Count);
                Assert.AreEqual(10, builder.SkillLevel);
            }

            private void AddRemoveEmployeePositiveTest<T>(Func<OfficeData, bool> tryHireAny, Action<OfficeData, T> fireAny, Func<DivisionsData, IReadOnlyDivision> getDivison) where T : EmployeeData
            {
                PrepareAnyTest(out OfficeData office);
                OfficeInfo officeWithManyEmployees = DB.Instance.OfficeInfo.Find(x => x.Data.MaximumEmployees > 2).Data;
                if (officeWithManyEmployees == null)
                {
                    Debug.LogError("Can't do test because office with employees > 2 isn't exist");
                    throw new System.InvalidProgramException();
                }
                office.TryReplaceInfo(officeWithManyEmployees.Id);
                int maxEmployees = ((OfficeInfo)office.Info).MaximumEmployees;
                IReadOnlyDivision division = getDivison.Invoke(office.Divisions);
                Assert.IsTrue(tryHireAny.Invoke(office));
                Assert.IsTrue(tryHireAny.Invoke(office));
                Assert.IsTrue(tryHireAny.Invoke(office));
                Assert.AreEqual(0, division.Employees[0].Id);
                Assert.AreEqual(1, division.Employees[1].Id);
                Assert.AreEqual(2, division.Employees[2].Id);
                fireAny.Invoke(office, (T)division.Employees[1]);
                Assert.IsTrue(tryHireAny.Invoke(office));
                Assert.AreEqual(2, division.Employees[1].Id);
                Assert.AreEqual(3, division.Employees[2].Id);
                fireAny.Invoke(office, (T)division.Employees[0]);
                fireAny.Invoke(office, (T)division.Employees[0]);
                fireAny.Invoke(office, (T)division.Employees[0]);

                Assert.IsTrue(tryHireAny.Invoke(office));
                for (int i = 0; i < maxEmployees - 1; ++i)
                {
                    Assert.IsTrue(tryHireAny.Invoke(office));
                }
                Assert.IsFalse(tryHireAny.Invoke(office));

                Assert.AreEqual(maxEmployees, division.Employees.Count);

                for (int i = maxEmployees - 1; i >= 0; --i)
                {
                    fireAny.Invoke(office, (T)division.Employees[i]);
                }
                Assert.AreEqual(0, division.Employees.Count);
                Assert.IsTrue(tryHireAny.Invoke(office));
            }
            private void AddRemoveEmployeeNegativeTest<T>(Func<OfficeData, bool> tryHireAny, Action<OfficeData, T> fireAny, Func<DivisionsData, IReadOnlyDivision> getDivison) where T : EmployeeData
            {
                PrepareAnyTest(out OfficeData office);
                IReadOnlyDivision division = getDivison.Invoke(office.Divisions);
                Assert.AreEqual(0, division.Employees.Count);
                Assert.IsFalse(tryHireAny.Invoke(office));
                Assert.AreEqual(0, division.Employees.Count);
                Assert.IsTrue(office.TryReplaceInfo(0));
                fireAny.Invoke(office, null);
                Assert.AreEqual(0, division.Employees.Count);
                Assert.IsTrue(tryHireAny.Invoke(office));
                Assert.AreEqual(1, division.Employees.Count);
                Assert.AreEqual(0, division.Employees[0].Id);
                fireAny.Invoke(office, null);
                Assert.AreEqual(1, division.Employees.Count);
                Assert.AreEqual(0, division.Employees[0].Id);
            }
            private void AddRemoveSingleEmployeePositiveTest(Func<OfficeData, bool> tryHireSingle, Action<OfficeData> fireSingle, Func<DivisionsData, IReadOnlyRole> getRole)
            {
                PrepareAnyTest(out OfficeData office);
                office.TryReplaceInfo(0);
                IReadOnlyRole role = getRole.Invoke(office.Divisions);
                Assert.IsFalse(role.IsEmployeeHired());
                Assert.IsTrue(tryHireSingle.Invoke(office));
                Assert.IsTrue(role.IsEmployeeHired());
                Assert.IsFalse(tryHireSingle.Invoke(office));
                fireSingle.Invoke(office);
                Assert.IsFalse(role.IsEmployeeHired());
                Assert.IsTrue(tryHireSingle.Invoke(office));
                Assert.IsFalse(tryHireSingle.Invoke(office));
                Assert.IsTrue(role.IsEmployeeHired());
            }
            private void AddRemoveSingleEmployeeNegativeTest(Func<OfficeData, bool> tryHireSingle, Action<OfficeData> fireSingle, Func<DivisionsData, IReadOnlyRole> getRole)
            {
                PrepareAnyTest(out OfficeData office);
                IReadOnlyRole role = getRole.Invoke(office.Divisions);
                Assert.AreEqual(0, office.Divisions.GetEmployeesCount());
                Assert.IsFalse(tryHireSingle.Invoke(office));
                fireSingle.Invoke(office);
                Assert.IsFalse(tryHireSingle.Invoke(office));
                Assert.IsFalse(role.IsEmployeeHired());
                Assert.IsTrue(office.TryReplaceInfo(0));
                Assert.IsTrue(tryHireSingle.Invoke(office));
                Assert.IsTrue(role.IsEmployeeHired());
                Assert.IsFalse(tryHireSingle.Invoke(office));
                Assert.IsFalse(tryHireSingle.Invoke(office));
                fireSingle.Invoke(office);
                fireSingle.Invoke(office);
                Assert.IsTrue(tryHireSingle.Invoke(office));
                Assert.IsFalse(tryHireSingle.Invoke(office));
                Assert.IsTrue(role.IsEmployeeHired());
            }

            private void PrepareAnyTest(out OfficeData office)
            {
                AssetLoader.InitInstances();
                GameData.SetData(new());
                office = GameData.Data.CompanyData.OfficeData;
            }
            #endregion methods
        }
    }
}