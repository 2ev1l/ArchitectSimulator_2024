using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class HumanInfoSOSet : DBSOSet<HumanInfoSO>
    {
        #region fields & properties
        public IReadOnlyList<HumanInfoSO> MaleList
        {
            get
            {
                maleList ??= Data.Where(x => x.Data.Male).ToList();
                return maleList;
            }
        }
        [System.NonSerialized] private List<HumanInfoSO> maleList = null;
        public IReadOnlyList<HumanInfoSO> FemaleList
        {
            get
            {
                femaleList ??= Data.Where(x => !x.Data.Male).ToList();
                return maleList;
            }
        }
        [System.NonSerialized] private List<HumanInfoSO> femaleList = null;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}