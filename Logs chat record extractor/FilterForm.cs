using System;
using System.Collections;
using System.Windows.Forms;

namespace Logs_chat_record_extractor
{
    public partial class FilterForm : Form
    {
        private readonly CheckBox[] _checkBoxArr = new CheckBox[EnumHandler.GetEnumCount() - 1];

        private MainForm MainForm { get; set; }

        public ArrayList ChatsBeanList { get; set; }

        public FilterForm()
        {
            InitializeComponent();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            MainForm = (MainForm) Owner;
            for (var i = 0; i < _checkBoxArr.Length; i++)
            {
                var useI = i;
                if (i >= (int) ChatType.TellToMe)
                {
                    useI++;
                }

                _checkBoxArr[i] = new CheckBox {Text = EnumHandler.GetNameFromEnum((ChatType) useI)};
                flowLayoutPanel1.Controls.Add(_checkBoxArr[i]);
                _checkBoxArr[i].Checked = EnumHandler.IsChecked[i];

                // 这些还没有做完嘤嘤嘤，先锁上
                if (useI == (int) ChatType.Alliance || useI == (int) ChatType.Beginner ||
                    useI == (int) ChatType.PvpTeam || useI == (int) ChatType.LinkShell1 ||
                    useI == (int) ChatType.LinkShell2 || useI == (int) ChatType.LinkShell3 ||
                    useI == (int) ChatType.LinkShell4 || useI == (int) ChatType.LinkShell5 ||
                    useI == (int) ChatType.LinkShell6 || useI == (int) ChatType.LinkShell7 ||
                    useI == (int) ChatType.LinkShell8 || useI == (int) ChatType.CwLinkShell2 ||
                    useI == (int) ChatType.CwLinkShell3 || useI == (int) ChatType.CwLinkShell4 ||
                    useI == (int) ChatType.CwLinkShell5 || useI == (int) ChatType.CwLinkShell6 ||
                    useI == (int) ChatType.CwLinkShell7 || useI == (int) ChatType.CwLinkShell4)
                {
                    _checkBoxArr[i].Enabled = false;
                }
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < EnumHandler.IsChecked.Length; i++)
            {
                EnumHandler.IsChecked[i] = _checkBoxArr[i].Checked;
            }

            var list = new ArrayList();

            foreach (ChatsBean chatsBean in ChatsBeanList)
            {
                var useI = chatsBean.ChatInfo.ChatType;
                if (chatsBean.ChatInfo.ChatType > ChatType.TellToMe)
                {
                    useI--;
                }

                chatsBean.Show = EnumHandler.IsChecked[(int) useI];
                list.Add(chatsBean);
            }

            Hide();
            MainForm.ChatsBeanList = list;
            MainForm.RefreshRichTextBox();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < EnumHandler.IsChecked.Length; i++)
            {
                _checkBoxArr[i].Checked = EnumHandler.IsChecked[i];
            }

            Hide();
        }

        /// <summary>
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var checkBox in _checkBoxArr)
            {
                if (checkBox.Enabled) checkBox.Checked = false;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (var checkBox in _checkBoxArr)
            {
                if (checkBox.Enabled) checkBox.Checked = true;
            }
        }
    }
}