using EditorCustom.Attributes;
using Game.Events;
using Game.Serialization.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay.Computer.BuildingConstructionApp
{
    public class StartedTaskConstructionItem : StartedConstructionItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnBeforeBuildCompleted()
        {
            base.OnBeforeBuildCompleted();
            int reference = Context.BaseId;
            ConstructionTasksData tasks = GameData.Data.CompanyData.ConstructionTasks;
            tasks.StartedTasks.Exists(x => x.BlueprintBaseIdReference == reference, out ConstructionTaskData task);
            if (task == null)
            {
                InfoRequest.GetErrorRequest(401).Send();
                return;
            }
            tasks.TryCompleteTask(task.Id);
        }
        #endregion methods
    }
}