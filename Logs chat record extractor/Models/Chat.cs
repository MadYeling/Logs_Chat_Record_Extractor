using System;

namespace Logs_chat_record_extractor.Models
{
    public class Chat
    {
        /// <summary>
        /// 聊天信息
        /// </summary>
        public ChatInfo ChatInfo { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 玩家名称
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }

        public override string ToString()
        {
            if (ChatInfo.ChatType == ChatType.Motion)
            {
                return TimeToString() + Context;
            }

            return TimeToString() + NameToString() + Context;
        }

        /// <summary>
        /// 将聊天类型转文字
        /// </summary>
        /// <param name="isPrivate">是否为固定显示</param>
        /// <returns>转换的文字</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出数组界限</exception>
        public string ChatTypeToString(bool isPrivate)
        {
            switch (ChatInfo.ChatType)
            {
                case ChatType.Party:
                case ChatType.Alliance:
                case ChatType.PvpTeam:
                case ChatType.Speak:
                case ChatType.Yell:
                case ChatType.Shout:
                case ChatType.Tell:
                case ChatType.TellToOther:
                case ChatType.Motion:
                case ChatType.MotionCustom:
                    return $"[{ChatTypeHandler.ChatTypeToName(ChatInfo.ChatType)}]";
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
                    return isPrivate ? $"[{ChatTypeHandler.ChatTypeToName(ChatInfo.ChatType)}]" : "";
                case ChatType.Beginner:
                    return isPrivate ? "[新人]" : "";
                case ChatType.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "";
        }

        /// <summary>
        /// 处理玩家名称
        /// </summary>
        /// <returns>处理后的名称</returns>
        public string NameToString()
        {
            switch (ChatInfo.ChatType)
            {
                case ChatType.Yell:
                case ChatType.Speak:
                case ChatType.Shout:
                    return PlayerName + " : ";
                case ChatType.Party:
                case ChatType.Alliance:
                    return "(" + PlayerName + ") ";
                case ChatType.Tell:
                    return ">>" + PlayerName + " : ";
                case ChatType.TellToOther:
                    return PlayerName + ">>";
                case ChatType.Motion:
                    return "";
                case ChatType.MotionCustom:
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
                    return ChatTypeToString(true) + "<" + PlayerName + "> ";
                case ChatType.Beginner:
                    return ChatTypeToString(true) + PlayerName + ": ";
                case ChatType.End:
                    return "";
                default:
                    return PlayerName;
            }
        }

        /// <summary>
        /// 处理时间
        /// </summary>
        /// <returns></returns>
        public string TimeToString()
        {
            return "[" + Time + "]";
        }
    }
}