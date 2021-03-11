using System;

namespace Logs_chat_record_extractor
{
    public static class EnumHandler
    {
        public static readonly bool[] IsChecked = new bool[GetEnumCount() - 1];

        public static int GetEnumCount()
        {
            return (int) ChatType.End;
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
                case ChatType.TellToMe:
                case ChatType.TellToOther: return "悄悄话";
                case ChatType.Motion: return "情感动作";
                case ChatType.LinkShell1: return "通讯贝1";
                case ChatType.LinkShell2: return "通讯贝2";
                case ChatType.LinkShell3: return "通讯贝3";
                case ChatType.LinkShell4: return "通讯贝4";
                case ChatType.LinkShell5: return "通讯贝5";
                case ChatType.LinkShell6: return "通讯贝6";
                case ChatType.LinkShell7: return "通讯贝7";
                case ChatType.LinkShell8: return "通讯贝8";
                case ChatType.CwLinkShell1: return "跨服通讯贝1";
                case ChatType.CwLinkShell2: return "跨服通讯贝2";
                case ChatType.CwLinkShell3: return "跨服通讯贝3";
                case ChatType.CwLinkShell4: return "跨服通讯贝4";
                case ChatType.CwLinkShell5: return "跨服通讯贝5";
                case ChatType.CwLinkShell6: return "跨服通讯贝6";
                case ChatType.CwLinkShell7: return "跨服通讯贝7";
                case ChatType.CwLinkShell8: return "跨服通讯贝8";
                case ChatType.Beginner: return "新人频道";
                case ChatType.End:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(chatType), chatType, null);
            }
        }
    }
}