using System;
using System.Collections;
using System.Windows.Forms;

namespace Logs_chat_record_extractor
{
    public partial class MainForm : Form
    {
        public ArrayList ChatsBeanList;

        private FilterForm _filterForm;
        private bool _showTime = true;
        private bool _showHead;

        public string MyTitle;

        public MainForm()
        {
            InitializeComponent();
            FormClosed += MainForm_FormClosed;
            KeyDown += mainForm_KeyDown;
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// 加载窗口的时候就把内容显示出来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = MyTitle;
            RefreshRichTextBox();
        }

        /// <summary>
        /// 刷新UI
        /// </summary>
        public void RefreshRichTextBox()
        {
            richTextBox1.Text = "";
            richTextBox1.Enabled = false;
            foreach (ChatsBean cb in ChatsBeanList)
            {
                if (!cb.Show) continue;
                richTextBox1.SelectionColor = cb.ChatInfo.ChatColor;
                if (_showTime) richTextBox1.AppendText(cb.TimeToString());
                if (_showHead) richTextBox1.AppendText(cb.HeadToString(false));
                richTextBox1.AppendText(cb.NameToString());
                richTextBox1.AppendText(cb.Context);
                richTextBox1.AppendText("\r\n");
            }

            // 从顶部开始浏览
            richTextBox1.Select(0, 0);
            richTextBox1.ScrollToCaret();
            richTextBox1.Enabled = true;
        }

        /// <summary>
        /// 当关闭此窗口时，显示被隐藏的前一个窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner.Show();
        }

        /// <summary>
        /// 聊天过滤器点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filterForm == null)
            {
                _filterForm = new FilterForm {ChatsBeanList = ChatsBeanList};
            }

            _filterForm.Show(this);
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showTime = !_showTime;
            timeToolStripMenuItem.Checked = _showTime;
            RefreshRichTextBox();
        }

        private void headToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showHead = !_showHead;
            headToolStripMenuItem.Checked = _showHead;
            RefreshRichTextBox();
        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // 组合键
            if (e.KeyCode != Keys.F || e.Modifiers != Keys.Control) return;
            var findForm = new FindForm(richTextBox1);
            findForm.Show(this);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var findForm = new FindForm(richTextBox1);
            findForm.Show(this);
        }
    }
}