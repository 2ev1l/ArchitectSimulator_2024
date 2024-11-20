using Game.Serialization.World;
using Game.UI.Overlay.Computer.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.DesignApp
{
    public class AvailableBlueprintTaskItem : AvailableBlueprintItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void UpdateUI()
        {
            base.UpdateUI();
            int id = Context.Id;
            if (GameData.Data.CompanyData.ConstructionTasks.StartedTasks.Exists(x => x.Info.BlueprintInfo.Id == id, out ConstructionTaskData task))
            {
                ChangeName($"B-{Context.Id:000} | {task.HumanInfo.Name}");
            }
        }
        protected override void OnBlueprintAdded(BlueprintData added)
        {
            base.OnBlueprintAdded(added);
            int blueprintId = Context.Id;
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            if (tasks.StartedTasks.Exists(x => x.Info.BlueprintInfo.Id == blueprintId, out ConstructionTaskData task))
            {
                tasks.TryAcceptTask(task.Id, added.BaseId);
            }
        }
        #endregion methods
    }
}