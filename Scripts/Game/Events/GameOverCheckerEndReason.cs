using Game.DataBase;
using Game.UI.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public partial class GameOverChecker
    {
        [System.Serializable]
        public abstract class EndReason
        {
            #region fields & properties

            #endregion fields & properties

            #region methods
            public abstract string GetReason();
            #endregion methods
        }
        
        [System.Serializable]
        public class BillsReason : EndReason
        {
            public override string GetReason()
            {
                return LanguageLoader.GetTextByType(TextType.Game, 151);
            }
        }
        [System.Serializable]
        public class MoodReason : EndReason
        {
            public override string GetReason()
            {
                return LanguageLoader.GetTextByType(TextType.Game, 152);
            }
        }
        [System.Serializable]
        public class AgeReason : EndReason
        {
            public override string GetReason()
            {
                return LanguageLoader.GetTextByType(TextType.Game, 157);
            }
        }
        [System.Serializable]
        public class SaturationReason : EndReason
        {
            public override string GetReason()
            {
                return LanguageLoader.GetTextByType(TextType.Game, 390);
            }
        }
    }
}