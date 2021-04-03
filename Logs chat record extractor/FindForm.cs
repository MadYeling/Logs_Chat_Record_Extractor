using System;
using System.Windows.Forms;

namespace Logs_chat_record_extractor
{
    public partial class FindForm : Form
    {
        /// <summary>
        /// 是否大小写
        /// </summary>
        private static bool _isMatchCase = true;

        /// <summary>
        /// 是否循环
        /// </summary>
        private static bool _isCycle;

        /// <summary>
        /// 搜索方向，true为向上，false为向下
        /// </summary>
        private static bool _direction;

        /// <summary>
        /// 上次搜索的内容
        /// </summary>
        private static string _lastFindText = "";

        /// <summary>
        /// 富文本框对象
        /// </summary>
        private readonly RichTextBox _richTextBox;

        /// <summary>
        /// 含参的构造函数
        /// </summary>
        /// <param name="richTextBox">富文本框</param>
        public FindForm(RichTextBox richTextBox)
        {
            InitializeComponent();

            // 传入对象
            _richTextBox = richTextBox;

            // 注册关闭事件
            FormClosed += FindForm_Closed;

            // 先把按钮状态设置一下
            checkBox1.Checked = _isMatchCase;
            checkBox2.Checked = _isCycle;
            radioButton1.Checked = _direction;
            radioButton2.Checked = !_direction;

            string clipboardText;

            // 三个优先级，最优先填充选择内容，其次上次搜索内容，最后剪切板内容
            // 都没有就把按钮禁掉
            if (!"".Equals(richTextBox.SelectedText))
            {
                textBox1.Text = richTextBox.SelectedText;
            }
            else if (!"".Equals(_lastFindText))
            {
                textBox1.Text = _lastFindText;
            }
            // 当剪切板里面的东西太长的时候，大概是复制到了奇怪的东西，就不填进去了
            else if (Clipboard.ContainsText(TextDataFormat.Text) &&
                     (clipboardText = Clipboard.GetText(TextDataFormat.Text)).Length < 10)
            {
                textBox1.Text = clipboardText;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        /// <summary>
        /// 匹配大小写选择框 状态变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isMatchCase = checkBox1.Checked;
        }

        /// <summary>
        /// 循环选择框 状态变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _isCycle = checkBox2.Checked;
        }

        /// <summary>
        /// 搜索文本框 文本变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !"".Equals(textBox1.Text);
        }

        /// <summary>
        /// 取消按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 查找按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            MyFind(textBox1.Text, false);
        }

        /// <summary>
        /// 向上单选按钮 状态变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _direction = radioButton1.Checked;
        }

        /// <summary>
        /// 向下单选按钮 状态变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _direction = !radioButton2.Checked;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="str">查找的内容</param>
        /// <param name="seekAgain">是否为第二次寻找</param>
        private void MyFind(string str, bool seekAgain)
        {
            // 获取光标位置
            var rboxL = _richTextBox.SelectionStart;
            // 从上往下搜索需要以选中内容的结尾作为开始，不然会一直搜索到同一个内容
            var downStart = _richTextBox.SelectionStart + _richTextBox.SelectionLength;
            // 使用自带方法进行查找
            // 这是一个带有数个三元运算的神奇语句
            // 其实并不是很难读懂嘛
            var index = _richTextBox.Find(str,
                _direction ? 0 : downStart,
                _direction ? rboxL : _richTextBox.MaxLength,
                (_direction ? RichTextBoxFinds.Reverse : RichTextBoxFinds.None) |
                (_isMatchCase ? RichTextBoxFinds.MatchCase : RichTextBoxFinds.None)
            );

            // 找到内容，给予焦点
            if (index > -1)
            {
                _richTextBox.Focus();
            }
            // 如果开启循环并且没有搜索到需要的内容，就把指针根据方向设置挪到头或尾，然后递归
            else if (_isCycle && !seekAgain)
            {
                _richTextBox.SelectionStart = _direction ? _richTextBox.MaxLength : 0;
                // 将二次寻找标志符设置为true防止发生无限递归
                MyFind(str, true);
            }
            // 没有找到内容，发出提示
            else
            {
                MessageBox.Show($"找不到 \"{str}\"", "加载器", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindForm_Closed(object sender, FormClosedEventArgs e)
        {
            // 把搜索框里面的东西存一下
            _lastFindText = textBox1.Text;
        }
    }
}