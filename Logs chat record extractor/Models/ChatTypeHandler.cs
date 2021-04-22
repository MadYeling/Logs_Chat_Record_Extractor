using System;

namespace Logs_chat_record_extractor.Models
{
    public static class ChatTypeHandler
    {
        /// <summary>
        /// 是否显示此条聊天的数组
        /// 用于记录过滤器的选择状态
        /// </summary>
        private static readonly bool[] IsShowArray = new bool[GetEnumCount()];

        /// <summary>
        /// 获取数组的长度
        /// </summary>
        /// <returns></returns>
        public static int GetIsShowArrayLength()
        {
            return IsShowArray.Length;
        }

        /// <summary>
        /// 是否显示此类型的聊天记录
        /// </summary>
        /// <param name="chatType">聊天类型</param>
        /// <returns>是否显示</returns>
        public static bool IsShowThisChatType(ChatType chatType)
        {
            return IsShowThisChatType(ChatTypeToInt(chatType));
        }

        /// <summary>
        /// 是否显示此类型的聊天记录
        /// </summary>
        /// <param name="chatType">int类型的聊天类型</param>
        /// <returns>是否显示</returns>
        public static bool IsShowThisChatType(int chatType)
        {
            return IsShowArray[chatType];
        }

        /// <summary>
        /// 设置是否显示此类型的聊天记录
        /// </summary>
        /// <param name="chat">聊天信息</param>
        public static void SetIsShowThisChatType(Chat chat)
        {
            SetIsShowThisChatType(chat.ChatInfo.ChatType, chat.Show);
        }

        /// <summary>
        /// 设置是否显示此类型的聊天记录
        /// </summary>
        /// <param name="chatType">聊天类型</param>
        /// <param name="isShow">是否显示</param>
        public static void SetIsShowThisChatType(ChatType chatType, bool isShow)
        {
            SetIsShowThisChatType(ChatTypeToInt(chatType), isShow);
        }

        /// <summary>
        /// 设置是否显示此类型的聊天记录
        /// </summary>
        /// <param name="chatType">int类型的聊天类型</param>
        /// <param name="isShow">是否显示</param>
        public static void SetIsShowThisChatType(int chatType, bool isShow)
        {
            IsShowArray[chatType] = isShow;
        }

        /// <summary>
        /// 将聊天类型转换为中文
        /// </summary>
        /// <param name="chatType">聊天类型</param>
        /// <returns>聊天类型的中文名</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出枚举界限</exception>
        public static string ChatTypeToName(ChatType chatType)
        {
            switch (chatType)
            {
                case ChatType.Party: return "小队";
                case ChatType.Alliance: return "团队";
                case ChatType.FreeCompany: return "部队";
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

        /// <summary>
        /// 获取End枚举所在位置
        /// </summary>
        /// <returns></returns>
        private static int GetEnumCount()
        {
            return (int) ChatType.End;
        }

        /// <summary>
        /// 将聊天类型转换为int
        /// </summary>
        /// <param name="chatType">聊天类型</param>
        /// <returns>聊天类型对应数字</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出枚举范围</exception>
        private static int ChatTypeToInt(ChatType chatType)
        {
            switch (chatType)
            {
                case ChatType.MotionCustom:
                    return (int) ChatType.Motion;
                case ChatType.TellToOther:
                    return (int) ChatType.Tell;
                default:
                    return (int) chatType;
            }
        }
    }
}