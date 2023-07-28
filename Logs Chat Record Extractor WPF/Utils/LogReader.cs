using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using HandyControl.Controls;
using Logs_Chat_Record_Extractor_WPF.Models;

namespace Logs_Chat_Record_Extractor_WPF.Utils
{
    public class LogReader
    {
        private readonly List<Chat> _chatList;
        private static List<ChatInfo> _chatInfoList;
        private static string _timeZone = "+08:00";

        public LogReader()
        {
            InitChatInfoList();
            InitChecked();
            _chatList = new List<Chat>();
        }


        /// <summary>
        /// 初始化过滤器
        /// </summary>
        private static void InitChecked()
        {
            for (var i = 0; i < ChatTypeHandler.GetIsShowArrayLength(); i++)
            {
                if (i == (int)ChatType.Party || i == (int)ChatType.Speak ||
                    i == (int)ChatType.Yell || i == (int)ChatType.Alliance ||
                    i == (int)ChatType.Shout || i == (int)ChatType.Motion ||
                    i == (int)ChatType.Tell)
                {
                    ChatTypeHandler.SetIsShowThisChatType(i, true);
                }
            }
        }

        public async Task<IEnumerable<Chat>> LoadFile(string filePath)
        {
            _chatList.Clear();
            using (
                var sr = new StreamReader(
                    new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                )
            )
            {
                var line = await sr.ReadLineAsync();
                if (line != null)
                {
                    var tempZoneStr = line.Split('|')[1];
                    _timeZone = tempZoneStr.Substring(tempZoneStr.Length - 6);
                }

                // 读取文件
                do
                {
                    var chatInfo = IsAChatMessage(line);
                    if (chatInfo != null)
                    {
                        // 装载内容
                        _chatList.Add(LoadChat(chatInfo, line));
                    }
                } while ((line = await sr.ReadLineAsync()) != null);
            }

            return _chatList;
        }

        /// <summary>
        /// 校验读取的目标是否为聊天内容
        /// 是则返回聊天信息对象，不是则返回null
        /// 更新对6.0log支持，同时支持5.0log
        /// </summary>
        /// <param name="line">需要校验的内容</param>
        /// <returns>返回对象</returns>
        private static ChatInfo IsAChatMessage(string line)
        {
            return _chatInfoList.FirstOrDefault(chatInfo =>
                line.Contains(_timeZone + chatInfo.ChatCode) || line.Contains(_timeZone + chatInfo.ChatCode.ToUpper()));
        }

        /// <summary>
        /// 装载聊天信息
        /// </summary>
        /// <param name="chatInfo">聊天信息详情</param>
        /// <param name="line">内容</param>
        /// <returns>装载完成的聊天信息</returns>
        private static Chat LoadChat(ChatInfo chatInfo, string line)
        {
            // 替换小队前面的乱码字符
            // line = line.Replace("", "①").Replace("", "②")
            //     .Replace("", "③").Replace("", "④")
            //     .Replace("", "⑤").Replace("", "⑥")
            //     .Replace("", "⑦").Replace("", "⑧")
            //     // 替换物品乱码
            //     .Replace("", "")
            //     // 替换团本乱码
            //     .Replace("", "Ⓐ").Replace("", "Ⓑ")
            //     .Replace("", "Ⓒ");->（跨服前面的花瓣）
            var sp = line.Split('|');
            var time = sp[1].Substring(11, 5);
            var chat = new Chat
            {
                Show = ChatTypeHandler.IsShowThisChatType(chatInfo.ChatType),
                ChatInfo = chatInfo,
                Time = time,
                PlayerName = sp[3],
                Context = sp[4]
            };
            return chat;
        }


        private static void InitChatInfoList()
        {
            _chatInfoList = new List<ChatInfo>
            {
                new ChatInfo(ChatType.Speak, Color.FromRgb(249, 249, 249), "|000a|"),
                new ChatInfo(ChatType.Yell, Color.FromRgb(255, 255, 0), "|001e|"),
                new ChatInfo(ChatType.Shout, Color.FromRgb(255, 181, 122), "|000b|"),
                new ChatInfo(ChatType.Party, Color.FromRgb(122, 234, 255), "|000e|"),
                new ChatInfo(ChatType.CwLinkShell1, Color.FromRgb(220, 255, 144), "|0025|"),
                new ChatInfo(ChatType.CwLinkShell2, Color.FromRgb(220, 255, 144), "|0065|"),
                new ChatInfo(ChatType.CwLinkShell3, Color.FromRgb(220, 255, 144), "|0066|"),
                new ChatInfo(ChatType.CwLinkShell4, Color.FromRgb(220, 255, 144), "|0067|"),
                new ChatInfo(ChatType.CwLinkShell5, Color.FromRgb(220, 255, 144), "|0068|"),
                new ChatInfo(ChatType.CwLinkShell6, Color.FromRgb(220, 255, 144), "|0069|"),
                new ChatInfo(ChatType.CwLinkShell7, Color.FromRgb(220, 255, 144), "|006a|"),
                new ChatInfo(ChatType.CwLinkShell8, Color.FromRgb(220, 255, 144), "|006b|"),
                new ChatInfo(ChatType.LinkShell1, Color.FromRgb(220, 255, 144), "|0010|"),
                new ChatInfo(ChatType.LinkShell2, Color.FromRgb(220, 255, 144), "|0011|"),
                new ChatInfo(ChatType.LinkShell3, Color.FromRgb(220, 255, 144), "|0012|"),
                new ChatInfo(ChatType.LinkShell4, Color.FromRgb(220, 255, 144), "|0013|"),
                new ChatInfo(ChatType.LinkShell5, Color.FromRgb(220, 255, 144), "|0014|"),
                new ChatInfo(ChatType.LinkShell6, Color.FromRgb(220, 255, 144), "|0015|"),
                new ChatInfo(ChatType.LinkShell7, Color.FromRgb(220, 255, 144), "|0016|"),
                new ChatInfo(ChatType.LinkShell8, Color.FromRgb(220, 255, 144), "|0017|"),
                new ChatInfo(ChatType.Beginner, Color.FromRgb(220, 255, 144), "|001b|"),
                new ChatInfo(ChatType.Tell, Color.FromRgb(255, 196, 228), "|000d|"),
                new ChatInfo(ChatType.TellToOther, Color.FromRgb(255, 196, 228), "|000c|"),
                //官方情感动作
                new ChatInfo(ChatType.Motion, Color.FromRgb(198, 255, 243), "|001d|"),
                //自定义的情感动作
                new ChatInfo(ChatType.MotionCustom, Color.FromRgb(198, 255, 243), "|001c|"),
                new ChatInfo(ChatType.FreeCompany, Color.FromRgb(185, 226, 234), "|0018|"),
                new ChatInfo(ChatType.Alliance, Color.FromRgb(255, 146, 0), "|000f|"),
            };
        }
    }
}