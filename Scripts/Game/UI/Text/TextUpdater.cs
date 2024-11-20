using UnityEngine;
using TMPro;
using Universal.Serialization;
using Game.Serialization.Settings;
using EditorCustom.Attributes;

namespace Game.UI.Text
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextUpdater : MonoBehaviour
    {
        #region fields & properties
        protected TextMeshProUGUI Text
        {
            get
            {
                if (text == null) text = GetComponent<TextMeshProUGUI>();
                return text;
            }
        }
        private TextMeshProUGUI text;

        public bool UpdateFontStyle
        {
            get => updateFontStyle;
            set => updateFontStyle = value;
        }
        [SerializeField] private bool updateFontStyle = true;
        public bool UpdateSpacing
        {
            get => updateSpacing;
            set => updateSpacing = value;
        }
        [SerializeField] private bool updateSpacing = true;
        private static LanguageSettings Context => SettingsData.Data.LanguageSettings;
        #endregion fields & properties

        #region methods
        private void Start() => UpdateText();
        public void UpdateText()
        {
            if (UpdateFontStyle) SetStyle();
            if (UpdateSpacing) SetSpacing();
        }
        private void SetStyle()
        {
            Text.fontStyle = Context.FontStyle;
        }
        private void SetSpacing()
        {
            LanguageSettings context = Context;
            TextMeshProUGUI text = Text;
            text.lineSpacing = context.LineSpacing;
            text.characterSpacing = context.CharacterSpacing;
            text.wordSpacing = context.WordSpacing;
        }
        #endregion methods
    }
}