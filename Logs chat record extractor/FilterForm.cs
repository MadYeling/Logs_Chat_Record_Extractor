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

                LoadCheckBox(i);
                _checkBoxArr[i].Checked = EnumHandler.IsChecked[i];
                // 这些还没有做完嘤嘤嘤，先锁上
                if (useI == (int) ChatType.PvpTeam)
                {
                    _checkBoxArr[i].Enabled = false;
                }
            }

            g1AllSelect.Click += (o, args) => { AllSelect(0, 10); };
            g2AllSelect.Click += (o, args) => { AllSelect(10, 18); };
            g3AllSelect.Click += (o, args) => { AllSelect(18, 26); };
            g1Reverse.Click += (o, args) => { Reverse(0, 10); };
            g2Reverse.Click += (o, args) => { Reverse(10, 18); };
            g3Reverse.Click += (o, args) => { Reverse(18, 26); };
            g1NoSelect.Click += (o, args) => { NoSelect(0, 10); };
            g2NoSelect.Click += (o, args) => { NoSelect(10, 18); };
            g3NoSelect.Click += (o, args) => { NoSelect(18, 26); };
        }

        private void LoadCheckBox(int i)
        {
            var checkBox = Controls.Find("checkbox" + (i + 1), true)[0] as CheckBox;
            var useI = i;
            if (i >= (int) ChatType.TellToMe) useI++;
            if (checkBox == null) return;
            checkBox.Text = EnumHandler.GetNameFromEnum((ChatType) useI);
            _checkBoxArr[i] = checkBox;
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

        private void AllSelect(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (_checkBoxArr[i].Enabled) _checkBoxArr[i].Checked = true;
            }
        }

        private void Reverse(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (_checkBoxArr[i].Enabled) _checkBoxArr[i].Checked = !_checkBoxArr[i].Checked;
            }

            Console.WriteLine("执行了反选");
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