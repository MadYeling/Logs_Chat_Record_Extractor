﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// 显示时间
        /// </summary>
        private bool _showTime = true;

        /// <summary>
        /// 显示类型
        /// </summary>
        private bool _showHead = false;

        private List<Chat> _chatList;

        private static TextPointer _nextPosition;

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
            TimeMenuChecker.IsChecked = _showTime;
            TypeMenuChecker.IsChecked = _showHead;
            RichTextBox.FontFamily = new FontFamily(new Uri("pack://application:,,,/"),
                "./Fonts/#XIV AXIS Std ATK, ./Fonts/#AXIS Std R, ./Fonts/#华康青花黑 Std W5");
            RichTextBox.FontSize = 18;
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
            LoadingCircle.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;
            RichTextBox.Visibility = Visibility.Hidden;
            // 清空内容
            RichTextBox.Document.Blocks.Clear();
            // 锁定富文本框
            RichTextBox.IsEnabled = false;
            Title = Path.GetFileName(filePath);
            var chatList = await _logReader.LoadFile(filePath);
            _chatList = chatList;
            if (_chatList.Count == 0)
                MessageBox.Show("读取的LOGS中无任何聊天内容！", "注意", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshRichTextBox(chatList);
            Menu.Visibility = Visibility.Visible;
            RichTextBox.Visibility = Visibility.Visible;
            LoadingCircle.Visibility = Visibility.Hidden;
            _nextPosition = RichTextBox.Document.ContentStart;
        }

        /// <summary>
        /// 刷新富文本框
        /// </summary>
        public void RefreshRichTextBox(IEnumerable<Chat> chatList)
        {
            if (chatList == null)
            {
                return;
            }

            // 清空内容
            RichTextBox.Document.Blocks.Clear();
            // 锁定富文本框
            RichTextBox.IsEnabled = false;
            // 遍历读取聊天记录集合
            foreach (var chat in chatList.Where(chat => chat.Show))
            {
                var paragraph = new Paragraph();
                var content = new StringBuilder();
                // 是否显示时间
                if (_showTime) content.Append(chat.TimeToString());
                // 是否显示类型
                if (_showHead) content.Append(chat.ChatTypeToString(false));

                content.Append(chat.NameToString());
                content.Append(chat.Context);
                paragraph.Inlines.Add(content.ToString());
                paragraph.Foreground = new SolidColorBrush(chat.ChatInfo.ChatColor);
                paragraph.LineHeight = 0.1;
                paragraph.FontWeight = FontWeights.Black;
                RichTextBox.Document.Blocks.Add(paragraph);
            }

            // 从顶部开始浏览
            RichTextBox.ScrollToHome();
            // 解锁富文本框
            RichTextBox.IsEnabled = true;
        }

        private void Time_OnChecked(object sender, RoutedEventArgs e)
        {
            _showTime = true;
            RefreshRichTextBox(_chatList);
        }

        private void Time_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _showTime = false;
            RefreshRichTextBox(_chatList);
        }

        private void Type_OnChecked(object sender, RoutedEventArgs e)
        {
            _showHead = true;
            RefreshRichTextBox(_chatList);
        }

        private void Type_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _showHead = false;
            RefreshRichTextBox(_chatList);
        }

        private void Quit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void Filter_OnClick(object sender, RoutedEventArgs e)
        {
            new Filter(_chatList, this).Show();
        }

        private void FindNext_OnClick(object sender, RoutedEventArgs e)
        {
            var word = SearchBar.Text;
            if (SearchBar.Text == "")
            {
                return;
            }
            var start = FindWord.FindWordPosition(_nextPosition, word);
            if (start == null)
            {
                _nextPosition = RichTextBox.Document.ContentStart;
                start = FindWord.FindWordPosition(_nextPosition, word);
            }

            var end = start.GetPositionAtOffset(word.Length);
            _nextPosition = end;
            RichTextBox.Selection.Select(start, end);
            RichTextBox.Focus();
        }
    }
}