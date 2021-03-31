using System;
using System.Collections;
using System.Windows.Forms;
using Logs_chat_record_extractor.Models;

namespace Logs_chat_record_extractor
{
    public partial class FilterForm : Form
    {
        /// <summary>
        /// 复选框数组
        /// </summary>
        private readonly CheckBox[] _checkBoxArr = new CheckBox[ChatTypeHandler.IsChecked.Length];

        /// <summary>
        /// 主窗口对象
        /// </summary>
        private MainForm _mainForm;

        /// <summary>
        /// 聊天数组
        /// </summary>
        private readonly ArrayList _chatList;

        public FilterForm(ArrayList chatList)
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
                _checkBoxArr[i].Checked = ChatTypeHandler.IsChecked[i];
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
            checkBox.Text = ChatTypeHandler.GetNameFromEnum((ChatType) i);
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
            for (var i = 0; i < ChatTypeHandler.IsChecked.Length; i++)
            {
                ChatTypeHandler.IsChecked[i] = _checkBoxArr[i].Checked;
            }

            var newChatList = new ArrayList();
            foreach (Chat chat in _chatList)
            {
                chat.Show = ChatTypeHandler.IsChecked[ChatTypeHandler.ChatTypeToInt(chat.ChatInfo.ChatType)];
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
            for (var i = 0; i < ChatTypeHandler.IsChecked.Length; i++)
            {
                _checkBoxArr[i].Checked = ChatTypeHandler.IsChecked[i];
            }

            Hide();
        }

        private void AllSelect(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (_checkBoxArr[i].Enabled) _checkBoxArr[i].Checked = true;
            }
        }

        private void NoSelect(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (_checkBoxArr[i].Enabled) _checkBoxArr[i].Checked = false;
            }
        }
    }
}