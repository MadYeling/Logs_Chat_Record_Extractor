using System;

namespace Logs_chat_record_extractor.Models
{
    public static class ChatTypeHandler
    {
        public static readonly bool[] IsChecked = new bool[GetEnumCount()];

        public static int GetEnumCount()
        {
            return (int) ChatType.End;
        }

        public static int ChatTypeToInt(ChatType chatType)
        {
            switch (chatType)
            {
                case ChatType.Party:
                    return 0;
                case ChatType.Alliance:
                    return 1;
                case ChatType.FreeCompany:
                    return 2;
                case ChatType.PvpTeam:
                    return 3;
                case ChatType.Speak:
                    return 4;
                case ChatType.Yell:
                    return 5;
                case ChatType.Shout:
                    return 6;
                case ChatType.Tell:
                    return 7;
                case ChatType.Motion:
                    return 8;
                case ChatType.Beginner:
                    return 9;
                case ChatType.LinkShell1:
                    return 10;
                case ChatType.LinkShell2:
                    return 11;
                case ChatType.LinkShell3:
                    return 12;
                case ChatType.LinkShell4:
                    return 13;
                case ChatType.LinkShell5:
                    return 14;
                case ChatType.LinkShell6:
                    return 15;
                case ChatType.LinkShell7:
                    return 16;
                case ChatType.LinkShell8:
                    return 17;
                case ChatType.CwLinkShell1:
                    return 18;
                case ChatType.CwLinkShell2:
                    return 19;
                case ChatType.CwLinkShell3:
                    return 20;
                case ChatType.CwLinkShell4:
                    return 21;
                case ChatType.CwLinkShell5:
                    return 22;
                case ChatType.CwLinkShell6:
                    return 23;
                case ChatType.CwLinkShell7:
                    return 24;
                case ChatType.CwLinkShell8:
                    return 25;
                case ChatType.End:
                    return 26;
                case ChatType.MotionCustom:
                    return 8;
                case ChatType.TellToOther:
                    return 7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chatType), chatType, null);
            }
        }

        public static string GetNameFromEnum(ChatType chatType)
        {
            switch (chatType)
            {
                case ChatType.Party: return "小队";
                case ChatType.Alliance: return "团队";
                case ChatType.FreeCompany: return "部队";
                case ChatType.PvpTeam: return "PVP战队";
                case ChatType.Speak: return "说话";
                case ChatType.Yell: return "呼喊";
                case ChatType.Shout: return "喊话";
                case ChatType.Tell:
                case ChatType.TellToOther:
                    return "悄悄话";
                case ChatType.Motion:
                case ChatType.MotionCustom:
                    return "情感动作";
                case ChatType.LinkShell1: return "通讯贝1";
                case ChatType.LinkShell2: return "通讯贝2";
                case ChatType.LinkShell3: return "通讯贝3";
                case ChatType.LinkShell4: return "通讯贝4";
                case ChatType.LinkShell5: return "通讯贝5";
                case ChatType.LinkShell6: return "通讯贝6";
                case ChatType.LinkShell7: return "通讯贝7";
                case ChatType.LinkShell8: return "通讯贝8";
                case ChatType.CwLinkShell1: return "跨服贝1";
                case ChatType.CwLinkShell2: return "跨服贝2";
                case ChatType.CwLinkShell3: return "跨服贝3";
                case ChatType.CwLinkShell4: return "跨服贝4";
                case ChatType.CwLinkShell5: return "跨服贝5";
                case ChatType.CwLinkShell6: return "跨服贝6";
                case ChatType.CwLinkShell7: return "跨服贝7";
                case ChatType.CwLinkShell8: return "跨服贝8";
                case ChatType.Beginner: return "新人频道";
                case ChatType.End:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(chatType), chatType, null);
            }
        }
    }
}