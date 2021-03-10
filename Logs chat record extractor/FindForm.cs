using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        /// 是否为第二次寻找
        /// </summary>
        private bool _seekAgain;


        public FindForm(string selectedText)
        {
            InitializeComponent();
            checkBox1.Checked = _isMatchCase;
            checkBox2.Checked = _isCycle;
            radioButton1.Checked = _direction;
            radioButton2.Checked = !_direction;
            string clipboardText;
            if (Clipboard.ContainsText(TextDataFormat.Text) &&
                (clipboardText = Clipboard.GetText(TextDataFormat.Text)).Length < 8)
            {
                textBox1.Text = clipboardText;
            }
            else if (!"".Equals(selectedText))
            {
                textBox1.Text = selectedText;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isMatchCase = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _isCycle = checkBox2.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !"".Equals(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainForm = (MainForm) Owner;
            var rbox = mainForm.MainFormRichTextBox;
            var str = textBox1.Text;
            MyFind(rbox, str);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _direction = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _direction = false;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="rbox">richbox对象</param>
        /// <param name="str">查找的内容</param>
        private void MyFind(RichTextBox rbox, string str)
        {
            // 获取光标位置
            var rboxL = rbox.SelectionStart;
            var downStart = rbox.SelectionStart + rbox.SelectionLength;
            // 使用自带方法进行查找
            // 这是一个带有超多三元运算的神奇语句
            // 其实并不是很难读懂嘛
            var index = rbox.Find(str,
                _direction ? 0 : downStart,
                _direction ? rboxL : rbox.MaxLength,
                (_direction ? RichTextBoxFinds.Reverse : RichTextBoxFinds.None) |
                (_isMatchCase ? RichTextBoxFinds.MatchCase : RichTextBoxFinds.None)
            );

            // 找到内容，给予焦点，并将二次寻找标志符设为false
            if (index > -1)
            {
                rbox.Focus();
                _seekAgain = false;
            }
            // 如果开启循环并且没有搜索到需要的内容，就把指针根据方向设置挪到头或尾
            else if (_isCycle && !_seekAgain)
            {
                rbox.SelectionStart = _direction ? rbox.MaxLength : 0;
                // 将二次寻找标志符设置为true防止发生无限递归
                _seekAgain = true;
                MyFind(rbox, str);
            }
            // 没有找到内容，发出提示，将二次寻找标志符设为false
            else
            {
                MessageBox.Show($"找不到 \"{str}\"", "ACT聊天记录加载器");
                _seekAgain = false;
            }
        }
    }
}