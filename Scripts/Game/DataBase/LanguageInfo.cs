using NUnit.Framework.Internal.Commands;
using System.ComponentModel.Design;
using UnityEngine;

namespace Game.DataBase
{
    [System.Serializable]
    public struct LanguageInfo
    {
        #region fields & properties
        [SerializeField][Min(-1)] private int id;
        [SerializeField] private TextType textType;
        /// <summary>
        /// This is exactly what you need
        /// </summary>
        public readonly string Text => GetTextByType(textType, id);
        public readonly string RawText => textType.GetRawText(id);

        private static readonly string commandStart = "<!";
        private static readonly string commandEnd = "!>";
        #endregion fields & properties

        #region methods
        public LanguageInfo(int id, TextType textType)
        {
            this.id = id;
            this.textType = textType;
        }
        /// <summary>
        /// Removes any tags <see cref="{}"/>
        /// </summary>
        public static string RemoveXMLTags(string text)
        {
            int startIndex = text.IndexOf("<");
            int endIndex = text.IndexOf(">");
            while (startIndex > -1)
            {
                text = text.Remove(startIndex, endIndex - startIndex + 1);
                startIndex = text.IndexOf("<");
                endIndex = text.IndexOf(">");
            }
            return text;
        }
        public static string TryReplaceText(string text, TextType textType)
        {
            text = TryReplaceEquationTag(text, textType);
            text = TryReplaceArrowTags(text, textType);
            return text;
        }
        private static string TryReplaceArrowTags(string text, TextType textType)
        {
            while (text.IndexOf(commandStart) > -1)
            {
                GetArrowCommands(text, out string replaceableText, out string command, out int value);
                string commandResult = DecryptArrowCommand(command, value, textType);
                text = text.Replace(replaceableText, commandResult);
            }
            return text;
        }
        private static void GetArrowCommands(string text, out string replacebaleText, out string command, out int value)
        {
            value = 0;
            command = "";
            replacebaleText = "";
            int arrowStartIndex = text.IndexOf(commandStart);
            int arrowEndIndex = text.IndexOf(commandEnd);
            if (arrowStartIndex < 0) return;

            replacebaleText = text.Substring(arrowStartIndex, arrowEndIndex - arrowStartIndex + 2);
            string subText = text.Substring(arrowStartIndex + 2, arrowEndIndex - arrowStartIndex - 2);
            int subTextLength = subText.Length;
            int equationPos = subText.IndexOf("=");
            if (equationPos < 0) return;

            command = subText[..equationPos];
            command = command.ToLower();
            try { value = System.Convert.ToInt32(subText.Substring(equationPos + 1, subTextLength - equationPos - 1)); }
            catch { }
        }
        private static string DecryptArrowCommand(string command, int value, TextType textType) => command switch
        {
            "get" => textType.GetRawText(value),
            _ => throw new System.NotImplementedException($"Wrong arrow command: '{command}' at {textType} {value}")
        };
        private static string TryReplaceEquationTag(string text, TextType textType)
        {
            if (text.Length > 0 && text[..1].Equals("="))
            {
                try
                {
                    int id = System.Convert.ToInt32(text[1..text.Length]);
                    text = GetTextByType(textType, id);
                }
                catch { }
            }
            return text;
        }
        public static string GetTextByType(TextType textType, int id)
        {
            string text = textType.GetRawText(id);
            text = TryReplaceText(text, textType);
            return text;
        }
        #endregion methods
    }
}