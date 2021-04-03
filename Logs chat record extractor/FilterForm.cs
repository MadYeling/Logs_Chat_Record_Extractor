using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Logs_chat_record_extractor.Models;

namespace Logs_chat_record_extractor
{
    public partial class FilterForm : Form
    {
        /// <summary>
        /// 数组长度
        /// </summary>
        private static readonly int IsShowArrLength = ChatTypeHandler.GetIsShowArrayLength();

        /// <summary>
        /// 复选框数组
        /// </summary>
        private readonly CheckBox[] _checkBoxArr = new CheckBox[IsShowArrLength];

        /// <summary>
        /// 主窗口对象
        /// </summary>
        private MainForm _mainForm;

        /// <summary>
        /// 聊天集合
        /// </summary>
        private readonly List<Chat> _chatList;

        public FilterForm(List<Chat> chatList)
        {
            InitializeComponent();
            _chatList = chatList;
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            // 获取主窗口对象
            _mainForm = (MainForm) Owner;

            // 加载复选框
            for (var i = 0; i < _checkBoxArr.Length; i++)
            {
                LoadCheckBox(i);
                _checkBoxArr[i].Checked = ChatTypeHandler.IsShowThisChatType(i);
                // 锁上没做好的复选框
                if (i == (int) ChatType.PvpTeam)
                {
                    _checkBoxArr[i].Enabled = false;
                }
            }

            // 注册各个全选按钮和取消全选按钮的点击事件
            g1AllSelect.Click += (o, args) => { AllSelect(0, 10); };
            g2AllSelect.Click += (o, args) => { AllSelect(10, 18); };
            g3AllSelect.Click += (o, args) => { AllSelect(18, 26); };
            g1NoSelect.Click += (o, args) => { NoSelect(0, 10); };
            g2NoSelect.Click += (o, args) => { NoSelect(10, 18); };
            g3NoSelect.Click += (o, args) => { NoSelect(18, 26); };
        }

        /// <summary>
        /// 将UI中的复选框装填到复选框数组当中
        /// </summary>
        /// <param name="i">位置</param>
        private void LoadCheckBox(int i)
        {
            if (!(Controls.Find("checkbox" + (i + 1), true)[0] is CheckBox checkBox)) return;
            checkBox.Text = ChatTypeHandler.ChatTypeToName((ChatType) i);
            _checkBoxArr[i] = checkBox;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 将储存复选框状态的数组修改为对应复选框的选择状态
            for (var i = 0; i < IsShowArrLength; i++)
            {
                ChatTypeHandler.SetIsShowThisChatType(i, _checkBoxArr[i].Checked);
            }

            var newChatList = new List<Chat>();
            foreach (var chat in _chatList)
            {
                chat.Show = ChatTypeHandler.IsShowThisChatType(chat.ChatInfo.ChatType);
                newChatList.Add(chat);
            }

            Hide();
            _mainForm.RefreshRichTextBox(newChatList);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < IsShowArrLength; i++)
            {
                _checkBoxArr[i].Checked = ChatTypeHandler.IsShowThisChatType(i);
            }

            Hide();
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
                if (_checkBoxArr[i].Enabled) _checkBoxArr[i].Checked = true;
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
                if (_checkBoxArr[i].Enabled) _checkBoxArr[i].Checked = false;
            }
        }
    }
}