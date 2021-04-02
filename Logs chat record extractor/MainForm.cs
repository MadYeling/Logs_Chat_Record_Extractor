using System;
using System.Collections;
using System.Windows.Forms;
using Logs_chat_record_extractor.Models;

namespace Logs_chat_record_extractor
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 聊天数组
        /// </summary>
        private readonly ArrayList _chatList;
        
        /// <summary>
        /// 标题
        /// </summary>
        private readonly string _myTitle;

        /// <summary>
        /// 过滤器窗口对象
        /// </summary>
        private FilterForm _filterForm;

        /// <summary>
        /// 显示时间
        /// </summary>
        private bool _showTime = true;

        /// <summary>
        /// 显示类型
        /// </summary>
        private bool _showHead;


        public MainForm(ArrayList chatList, string myTitle)
        {
            InitializeComponent();
            FormClosed += MainForm_FormClosed;
            KeyDown += mainForm_KeyDown;
            _chatList = chatList;
            _myTitle = myTitle;
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// 加载窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = _myTitle;
            RefreshRichTextBox(_chatList);
        }

        /// <summary>
        /// 刷新富文本框
        /// </summary>
        public void RefreshRichTextBox(ArrayList chatList)
        {
            // 清空内容
            richTextBox1.Text = "";
            // 锁定富文本框
            richTextBox1.Enabled = false;
            // 遍历读取聊天记录数组
            foreach (Chat cb in chatList)
            {
                // 是否显示这条记录
                if (!cb.Show) continue;
                // 设置文本颜色
                richTextBox1.SelectionColor = cb.ChatInfo.ChatColor;
                // 是否显示时间
                if (_showTime) richTextBox1.AppendText(cb.TimeToString());
                // 是否显示类型
                if (_showHead) richTextBox1.AppendText(cb.ChatTypeToString(false));

                richTextBox1.AppendText(cb.NameToString());
                richTextBox1.AppendText(cb.Context);
                richTextBox1.AppendText("\r\n");
            }

            // 从顶部开始浏览
            richTextBox1.Select(0, 0);
            richTextBox1.ScrollToCaret();
            // 解锁富文本框
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
        /// 菜单过滤器点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filterForm == null)
            {
                _filterForm = new FilterForm(_chatList);
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

        /// <summary>
        /// 菜单显示时间点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showTime = !_showTime;
            timeToolStripMenuItem.Checked = _showTime;
            RefreshRichTextBox(_chatList);
        }

        /// <summary>
        /// 菜单显示消息类型点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showHead = !_showHead;
            headToolStripMenuItem.Checked = _showHead;
            RefreshRichTextBox(_chatList);
        }

        /// <summary>
        /// 拦截组合键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+F
            if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
            {
                new FindForm(richTextBox1).Show(this);
            }
        }

        /// <summary>
        /// 菜单寻找选项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 弹出寻找窗口
            new FindForm(richTextBox1).Show(this);
        }
    }
}