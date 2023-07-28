using System.Drawing;
using System.Windows.Media;

namespace Logs_Chat_Record_Extractor_WPF.Models
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