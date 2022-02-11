using System;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TGUI.CoreLib.Services.Common;

namespace TGUI.CoreLib.Utils
{
    public static class SupportFunctions
    {
        public const string Activate = "/activate";
        public const string Ban = "/ban";

        private static readonly Regex commandParserReg1 = new Regex(@"^(/\w*)$");
        private static readonly Regex commandParserReg2 = new Regex(@"^(/\w*) (.*)$");
        public static bool TryParseCommand(string text, out (string command, string content) result)
        {
            if (text != null)
            {
                Match match1 = commandParserReg1.Match(text);
                Match match2 = commandParserReg2.Match(text);
                if (match1.Success)
                {
                    result = (match1.Groups[1].Value, string.Empty);
                    return true;
                }
                else if (match2.Success)
                {
                    result = (match2.Groups[1].Value, match2.Groups[2].Value);
                    return true;
                }
            }
            result = (string.Empty, string.Empty);
            return false;
        }
        public static string TextFormatingRecovering(MessageEntity[] Entities, string text)
        {
            if (Entities == null || text == null)
            {
                return text;
            }

            int offset = 0;
            Array.Sort(Entities, new EntityComparer());//TODO удостовериться, что они всегда сортированные
            foreach (MessageEntity entity in Entities)
            {
                string StartSymbols = string.Empty;
                string EndSymbols = string.Empty;
                switch (entity.Type)
                {
                    case MessageEntityType.Bold:
                        StartSymbols = "<b>";// "*";
                        EndSymbols = "</b>";// "*";
                        break;
                    case MessageEntityType.Italic:
                        StartSymbols = "<i>";// "_";
                        EndSymbols = "</i>";//"_";
                        break;
                    case MessageEntityType.Strikethrough:
                        StartSymbols = "<strike>";//"-";
                        EndSymbols = "</strike>";
                        break;
                    case MessageEntityType.Underline:
                        StartSymbols = "<u>";
                        EndSymbols = "</u>";
                        break;
                    case MessageEntityType.Code:
                        StartSymbols = "<code>";//"'"
                        EndSymbols = "</code>";
                        break;
                    case MessageEntityType.Pre:
                        StartSymbols = "<pre>";
                        EndSymbols = "</pre>";
                        break;
                    case MessageEntityType.TextLink:
                        StartSymbols = "<a href=\"" + entity.Url + "\">";
                        EndSymbols = "</a>";
                        break;
                    case MessageEntityType.TextMention:
                        StartSymbols = "<a href=\"tg://user?id=" + entity.User.Id.ToString() + "\">";
                        EndSymbols = "</a>";
                        break;
                }
                text = ApplyFormat(entity, StartSymbols, EndSymbols, text, ref offset);
            }
            return text;
        }
        private static string ApplyFormat(MessageEntity entity, string StartSymbols, string EndSymbols, string startText, ref int offset)
        {
            string newText = string.Empty;
            for (int iter = 0; iter < startText.Length; iter++)
            {
                if (iter == entity.Offset + offset)
                {
                    newText += StartSymbols;
                }
                if (iter == entity.Offset + entity.Length + offset)
                {
                    newText += EndSymbols;
                }
                newText += startText[iter];
            }
            if (entity.Offset + entity.Length + offset == startText.Length)
            {
                newText += EndSymbols;
            }

            offset += StartSymbols.Length;
            offset += EndSymbols.Length;
            return newText;
        }
    }
}
