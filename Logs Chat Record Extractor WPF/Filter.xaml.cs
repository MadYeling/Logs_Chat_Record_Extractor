using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Logs_Chat_Record_Extractor_WPF.Models;

namespace Logs_Chat_Record_Extractor_WPF
{
    /// <summary>
    /// Filter.xaml 的交互逻辑
    /// </summary>
    public partial class Filter
    {
        private List<Chat> _chatList;
        private readonly MainWindow _owner;

        /// <summary>
        /// 数组长度
        /// </summary>
        private static readonly int IsShowArrLength = ChatTypeHandler.GetIsShowArrayLength();

        /// <summary>
        /// 复选框数组
        /// </summary>
        private readonly CheckBox[] _checkBoxArr = new CheckBox[IsShowArrLength];

        public Filter(MainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            Init();
        }

        public void PrepareData(List<Chat> chatList)
        {
            _chatList = chatList;
        }

        private void Init()
        {
            // 加载复选框
            for (var i = 0; i < _checkBoxArr.Length; i++)
            {
                LoadCheckBox(i);
                _checkBoxArr[i].IsChecked = ChatTypeHandler.IsShowThisChatType(i);
            }

            NormalAllSelect.Click += (sender, args) => { AllSelect(0, 9); };
            LinkAllSelect.Click += (sender, args) => { AllSelect(9, 17); };
            CwLinkAllSelect.Click += (sender, args) => { AllSelect(17, 25); };
            NormalNoneSelect.Click += (sender, args) => { NoSelect(0, 9); };
            LinkNoneSelect.Click += (sender, args) => { NoSelect(9, 17); };
            CwLinkNoneSelect.Click += (sender, args) => { NoSelect(17, 25); };
        }

        /// <summary>
        /// 将UI中的复选框装填到复选框数组当中
        /// </summary>
        /// <param name="i">位置</param>
        private void LoadCheckBox(int i)
        {
            var normalCheckBox = new CheckBox()
            {
                Content = ChatTypeHandler.ChatTypeToName((ChatType)i)
            };
            _checkBoxArr[i] = normalCheckBox;
            if (i < 9)
            {
                NormalChannel.Children.Add(normalCheckBox);
            }
            else if (i < 17)
            {
                LinkShellChannel.Children.Add(normalCheckBox);
            }
            else if (i < 25)
            {
                CwLinkShellChannel.Children.Add(normalCheckBox);
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="start">起始复选框</param>
        /// <param name="end">结束复选框</param>
        private void AllSelect(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (_checkBoxArr[i].IsEnabled) _checkBoxArr[i].IsChecked = true;
            }
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="start">起始复选框</param>
        /// <param name="end">结束复选框</param>
        private void NoSelect(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (_checkBoxArr[i].IsEnabled) _checkBoxArr[i].IsChecked = false;
            }
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            // 将储存复选框状态的数组修改为对应复选框的选择状态
            for (var i = 0; i < IsShowArrLength; i++)
            {
                ChatTypeHandler.SetIsShowThisChatType(i, (bool)_checkBoxArr[i].IsChecked);
            }

            var newChatList = new List<Chat>();
            foreach (var chat in _chatList)
            {
                chat.Show = ChatTypeHandler.IsShowThisChatType(chat.ChatInfo.ChatType);
                newChatList.Add(chat);
            }

            Hide();
            _owner.RefreshRichTextBox(newChatList);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < IsShowArrLength; i++)
            {
                _checkBoxArr[i].IsChecked = ChatTypeHandler.IsShowThisChatType(i);
            }

            Hide();
        }
    }
}