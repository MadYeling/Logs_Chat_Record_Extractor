using System;

namespace Logs_chat_record_extractor
{
    public class ChatsBean
    {
        public ChatInfo ChatInfo { get; set; }
        public string Time { get; set; }
        public string PlayerName { get; set; }
        public string Context { get; set; }

        public bool Show { get; set; }

        public override string ToString()
        {
            switch (ChatInfo.ChatType)
            {
                case ChatType.Yell:
                case ChatType.Speak:
                case ChatType.Shout:
                case ChatType.Party:
                case ChatType.TellToOther:
                case ChatType.TellToMe:
                    return TimeToString() + NameToString() + Context;
                case ChatType.Motion:
                    return TimeToString() + Context;
                default:
                    return TimeToString() + NameToString() + Context;
            }
        }

        public string HeadToString(bool isPrivate)
        {
            switch (ChatInfo.ChatType)
            {
                case ChatType.Party:
                    return "[小队]";
                case ChatType.Alliance:
                    return "[团队]";
                case ChatType.FreeCompany:
                    return isPrivate ? "[部队]" : "";
                case ChatType.PvpTeam:
                    return "[PVP]";
                case ChatType.Speak:
                    return "[说话]";
                case ChatType.Yell:
                    return "[喊话]";
                case ChatType.Shout:
                    return "[呼喊]";
                case ChatType.TellToMe:
                    return "[悄悄话]";
                case ChatType.TellToOther:
                    return "[悄悄话]";
                case ChatType.Motion:
                    return "[情感动作]";
                case ChatType.LinkShell1:
                    return isPrivate ? "[通讯贝1]" : "";
                case ChatType.LinkShell2:
                    return isPrivate ? "[通讯贝2]" : "";
                case ChatType.LinkShell3:
                    return isPrivate ? "[通讯贝3]" : "";
                case ChatType.LinkShell4:
                    return isPrivate ? "[通讯贝4]" : "";
                case ChatType.LinkShell5:
                    return isPrivate ? "[通讯贝5]" : "";
                case ChatType.LinkShell6:
                    return isPrivate ? "[通讯贝6]" : "";
                case ChatType.LinkShell7:
                    return isPrivate ? "[通讯贝7]" : "";
                case ChatType.LinkShell8:
                    return isPrivate ? "[通讯贝8]" : "";
                case ChatType.CwLinkShell1:
                    return isPrivate ? "[跨服贝1]" : "";
                case ChatType.CwLinkShell2:
                    return isPrivate ? "[跨服贝2]" : "";
                case ChatType.CwLinkShell3:
                    return isPrivate ? "[跨服贝3]" : "";
                case ChatType.CwLinkShell4:
                    return isPrivate ? "[跨服贝4]" : "";
                case ChatType.CwLinkShell5:
                    return isPrivate ? "[跨服贝5]" : "";
                case ChatType.CwLinkShell6:
                    return isPrivate ? "[跨服贝6]" : "";
                case ChatType.CwLinkShell7:
                    return isPrivate ? "[跨服贝7]" : "";
                case ChatType.CwLinkShell8:
                    return isPrivate ? "[跨服贝8]" : "";
                case ChatType.Beginner: return "[新人频道]";
                case ChatType.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return "";
        }

        public string NameToString()
        {
            switch (ChatInfo.ChatType)
            {
                case ChatType.Yell:
                case ChatType.Speak:
                case ChatType.Shout:
                    return PlayerName + " : ";
                case ChatType.Party:
                    return "(" + PlayerName + ") ";
                case ChatType.TellToOther:
                    return ">>" + PlayerName + " : ";
                case ChatType.TellToMe:
                    return PlayerName + ">>";
                case ChatType.Motion:
                    return "";
                case ChatType.Alliance:
                    return PlayerName;
                case ChatType.PvpTeam:
                    return PlayerName;
                case ChatType.FreeCompany:
                case ChatType.LinkShell1:
                case ChatType.LinkShell2:
                case ChatType.LinkShell3:
                case ChatType.LinkShell4:
                case ChatType.LinkShell5:
                case ChatType.LinkShell6:
                case ChatType.LinkShell7:
                case ChatType.LinkShell8:
                case ChatType.CwLinkShell1:
                case ChatType.CwLinkShell2:
                case ChatType.CwLinkShell3:
                case ChatType.CwLinkShell4:
                case ChatType.CwLinkShell5:
                case ChatType.CwLinkShell6:
                case ChatType.CwLinkShell7:
                case ChatType.CwLinkShell8:
                    return HeadToString(true) + "<" + PlayerName + "> ";
                case ChatType.Beginner:
                    return PlayerName;
                case ChatType.End:
                    return "";
                default:
                    return PlayerName;
            }
        }

        public string TimeToString()
        {
            return "[" + Time + "]";
        }
    }
}