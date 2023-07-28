using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Logs_Chat_Record_Extractor_WPF.Models;
using Logs_Chat_Record_Extractor_WPF.Utils;

namespace Logs_Chat_Record_Extractor_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly Window _owner;
        private bool _killed = false;
        private LogReader _logReader;

        public MainWindow(Window owner)
        {
            InitializeComponent();
            Init();
            _owner = owner;
        }

        private void Init()
        {
            Closing += Window_Closing;
            var backgroundColor = ColorConverter.ConvertFromString("#555");
            if (backgroundColor != null)
                RichTextBox.Background = new SolidColorBrush((Color)backgroundColor);
            _logReader = new LogReader();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_killed) return;
            e.Cancel = true;
            Hide();
            _owner.Show();
        }

        public void Killed()
        {
            _killed = true;
            Close();
        }

        public async void LoadFile(string filePath)
        {
            // 清空内容
            RichTextBox.Document.Blocks.Clear();
            // 锁定富文本框
            RichTextBox.IsEnabled = false;
            Title = Path.GetFileName(filePath);
            var chatList = await _logReader.LoadFile(filePath);
            RefreshRichTextBox(chatList);
        }

        /// <summary>
        /// 刷新富文本框
        /// </summary>
        /// <param name="chatList">聊天集合</param>
        private void RefreshRichTextBox(IEnumerable<Chat> chatList)
        {
            // 遍历读取聊天记录集合
            foreach (var chat in chatList.Where(chat => chat.Show))
            {
                var paragraph = new Paragraph();
                var content = new StringBuilder();
                // 是否显示时间
                if (true) content.Append(chat.TimeToString());
                // 是否显示类型
                if (true) content.Append(chat.ChatTypeToString(false));

                content.Append(chat.NameToString());
                content.Append(chat.Context);
                paragraph.Inlines.Add(content.ToString());
                paragraph.Foreground = new SolidColorBrush(chat.ChatInfo.ChatColor);
                // paragraph.FontFamily = new FontFamily("file:///./Fonts/#XIV AXIS Std ATK");
                paragraph.FontFamily = new FontFamily(new Uri("pack://application:,,,/"),
                    "./Fonts/#XIV AXIS Std ATK, ./Fonts/#AXIS Std R, Microsoft Yahei");
                paragraph.FontSize = 18;
                paragraph.LineHeight = 0.1;
                RichTextBox.Document.Blocks.Add(paragraph);
            }

            // 从顶部开始浏览
            RichTextBox.ScrollToHome();
            // 解锁富文本框
            RichTextBox.IsEnabled = true;
        }
    }
}