using Game.DataBase;
using Game.Serialization.World;
using System.Collections.Generic;
using Universal.Collections.Generic;

namespace Game.UI.Collections
{
    internal class PlayerTaskList : InfinityItemListBase<TaskItem, PlayerTaskData>
    {
        #region fields & properties
        private static PlayerTasksData Context => GameData.Data.PlayerData.Tasks;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            Context.OnTaskStarted += UpdateListData;
            Context.OnTaskCompleted += UpdateListData;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            Context.OnTaskStarted -= UpdateListData;
            Context.OnTaskCompleted -= UpdateListData;
            base.OnDisable();
        }
        private void UpdateListData(PlayerTaskData _) => UpdateListData();
        public override void UpdateListData()
        {
            ItemList.UpdateListDefault(Context.StartedTasks, x => x);
        }

        #endregion methods
    }
}