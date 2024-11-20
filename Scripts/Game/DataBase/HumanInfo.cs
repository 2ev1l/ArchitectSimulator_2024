using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public class HumanInfo : DBInfo, IPreviewHandler
    {
        #region fields & properties
        private const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string Name => name;
        [Tooltip("Allowed only latin letters")][SerializeField] private string name;
        public Sprite PreviewSprite => previewSprite;
        [SerializeField] private Sprite previewSprite;
        public bool Male => male;
        [SerializeField] private bool male = true;
        #endregion fields & properties

        #region methods
        public override void OnValidate()
        {
            //ValidateName();
        }
        private void ValidateName()
        {
            int count = name.Length - 1;
            while (count > 0)
            {
                char c = name[count];
                if (!ALLOWED_CHARS.Contains(c) && !ALLOWED_CHARS.Contains(c.ToString().ToUpper()))
                {
                    name = name.Remove(count, 1);
                }
                count--;
            }
        }
        #endregion methods
    }
}