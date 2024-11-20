using TMPro;
using UnityEngine;
using Universal.Core;

namespace Universal.Serialization
{
    [System.Serializable]
    public class LanguageSettings : ICloneable<LanguageSettings>
    {
        #region fields & properties
        private const string DEFAULT_LANGUAGE = "English";
        [SerializeField] private string choosedLanguage = DEFAULT_LANGUAGE;

        [SerializeField][Range(1f, 4f)] private float subtitleSpeed = 1f;
        [SerializeField] private int lineSpacing = 0;
        [SerializeField] private int wordSpacing = 0;
        [SerializeField] private int characterSpacing = 0;
        [SerializeField] private FontStyles fontStyle = FontStyles.Normal;

        public string ChoosedLanguage { get => choosedLanguage; }
        public int LineSpacing { get => lineSpacing; }
        public int WordSpacing { get => wordSpacing; }
        public int CharacterSpacing { get => characterSpacing; }
        public FontStyles FontStyle { get => fontStyle; }
        /// <summary>
        /// 0.2f .. 4f
        /// </summary>
        public float SubtitleSpeed { get => subtitleSpeed; }
        #endregion fields & properties

        #region methods
        public void ResetLanguage() => choosedLanguage = DEFAULT_LANGUAGE;
        public LanguageSettings() { }
        public LanguageSettings(string choosedLanguage, int lineSpacing, int wordSpacing, int characterSpacing, FontStyles fontStyle, float subtitleSpeed)
        {
            this.choosedLanguage = choosedLanguage;
            this.lineSpacing = lineSpacing;
            this.wordSpacing = wordSpacing;
            this.characterSpacing = characterSpacing;
            this.fontStyle = fontStyle;
            this.subtitleSpeed = subtitleSpeed;
        }
        public LanguageSettings Clone()
        {
            return new(choosedLanguage, lineSpacing, wordSpacing, characterSpacing, fontStyle, subtitleSpeed);
        }
        #endregion methods
    }
}