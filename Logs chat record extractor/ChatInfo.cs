using System.Drawing;

namespace Logs_chat_record_extractor
{
    public class ChatInfo
    {
        public ChatType ChatType { get; set; }
        public Color ChatColor { get; set; }
        public string ChatCode { get; set; }

        public ChatInfo(ChatType chatType, Color chatColor, string chatCode)
        {
            ChatType = chatType;
            ChatColor = chatColor;
            ChatCode = chatCode;
        }

        public ChatInfo()
        {

        }
    }
}
