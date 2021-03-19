using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Logs_chat_record_extractor
{
    public partial class FormOpen : Form
    {
        private OpenFileDialog _openFileDialog1;

        private MainForm _mf;
        private readonly ArrayList _chatsBeanList;
        private static Hashtable _chatInfoMap;

        private string _mainFormTitle;

        public FormOpen()
        {
            InitializeComponent();
            InitChecked();
            LoadChatTable();
            _chatsBeanList = new ArrayList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _openFileDialog1 = new OpenFileDialog
            {
                Title = "请选择想要查询的log文件",
                Filter = "act日志文件(*.log)|*.log",
                RestoreDirectory = true
            };
            label1.Text = "就绪";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_openFileDialog1.ShowDialog() != DialogResult.OK) return;
            textBox1.Text = Path.GetFullPath(_openFileDialog1.FileName);
            _mainFormTitle = Path.GetFileName(_openFileDialog1.FileName);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var filePath = textBox1.Text;
            if (filePath.Length <= 3)
            {
                MessageBox.Show("请输入正确的文件目录");
                return;
            }

            label1.Text = "读取文件中，请稍候...";
            Application.DoEvents();
            // 将原先的list清理掉
            _chatsBeanList.Clear();
            using (var sr = new StreamReader(filePath))
            {
                string line;
                // 读取文件
                while ((line = sr.ReadLine()) != null)
                {
                    if (IsAChatMessage(line))
                    {
                        // 装载内容
                        _chatsBeanList.Add(LoadingSb(CheckType(line), line));
                    }
                }
            }

            _mf = new MainForm {ChatsBeanList = _chatsBeanList, MyTitle = _mainFormTitle};
            _mf.Show(this);
            Hide();
            label1.Text = "就绪";
        }

        /// <summary>
        /// 初始化过滤器
        /// </summary>
        private static void InitChecked()
        {
            for (var i = 0; i < EnumHandler.IsChecked.Length; i++)
            {
                var useI = i;
                if (i >= (int) ChatType.TellToMe)
                {
                    useI++;
                }

                if (useI == (int) ChatType.Party || useI == (int) ChatType.Speak ||
                    useI == (int) ChatType.Yell || useI == (int) ChatType.Alliance ||
                    useI == (int) ChatType.Shout || useI == (int) ChatType.Motion ||
                    useI == (int) ChatType.TellToOther)
                {
                    EnumHandler.IsChecked[i] = true;
                }
            }
        }

        /// <summary>
        /// 校验读取的目标是否为聊天内容
        /// </summary>
        /// <param name="line">需要校验的内容</param>
        /// <returns>是否为聊天记录</returns>
        private static bool IsAChatMessage(string line)
        {
            var isMeg = false;
            // 通过遍历来判断，避免每次新增一个聊天类型就要改一次
            foreach (var chatInfo in _chatInfoMap.Values)
            {
                isMeg |= line.Contains(((ChatInfo) chatInfo).ChatCode);
                if (isMeg)
                {
                    break;
                }
            }

            return isMeg;
        }

        /// <summary>
        /// 返回聊天类型
        /// </summary>
        /// <param name="line">内容</param>
        /// <returns>类型</returns>
        private static ChatType CheckType(string line)
        {
            var witchChatType = ChatType.Party;
            foreach (var chatInfo in _chatInfoMap.Values)
            {
                if (line.Contains(((ChatInfo) chatInfo).ChatCode))
                {
                    witchChatType = ((ChatInfo) chatInfo).ChatType;
                }
            }

            return witchChatType;
        }

        /// <summary>
        /// 装载聊天信息
        /// </summary>
        /// <param name="chatType">聊天类型</param>
        /// <param name="line">内容</param>
        /// <returns>装载完成的聊天信息</returns>
        private static ChatsBean LoadingSb(ChatType chatType, string line)
        {
            // 替换小队前面的乱码字符
            line = line.Replace("", "①").Replace("", "②")
                .Replace("", "③").Replace("", "④")
                .Replace("", "⑤").Replace("", "⑥")
                .Replace("", "⑦").Replace("", "⑧")
                // 替换物品乱码
                .Replace("", "")
                // 替换团本乱码
                .Replace("", "Ⓐ").Replace("", "Ⓑ")
                .Replace("","Ⓒ");
            var sp = line.Split('|');
            var time = sp[1].Substring(11, 5);
            var useI = chatType;
            if (chatType > ChatType.TellToMe)
            {
                useI--;
            }

            var chatsBean = new ChatsBean
            {
                Show = EnumHandler.IsChecked[(int) useI],
                ChatInfo = (ChatInfo) _chatInfoMap[chatType],
                Time = time,
                PlayerName = sp[3],
                Context = sp[4]
            };
            return chatsBean;
        }

        /// <summary>
        /// 初始化聊天信息表
        /// </summary>
        private static void LoadChatTable()
        {
            if (_chatInfoMap == null || _chatInfoMap.Count == 0)
            {
                _chatInfoMap = new Hashtable
                {
                    {
                        ChatType.Speak,
                        new ChatInfo(ChatType.Speak, Color.FromArgb(178, 178, 178), "+08:00|000a|")
                    },
                    {
                        ChatType.Yell,
                        new ChatInfo(ChatType.Yell, Color.FromArgb(178, 178, 0), "+08:00|001e|")
                    },
                    {
                        ChatType.Shout,
                        new ChatInfo(ChatType.Shout, Color.FromArgb(178, 116, 71), "+08:00|000b|")
                    },
                    {
                        ChatType.Party,
                        new ChatInfo(ChatType.Party, Color.FromArgb(71, 168, 178), "+08:00|000e|")
                    },
                    {
                        ChatType.CwLinkShell1,
                        new ChatInfo(ChatType.CwLinkShell1, Color.FromArgb(159, 191, 96), "+08:00|0025|")
                    },
                    {
                        ChatType.CwLinkShell8,
                        new ChatInfo(ChatType.CwLinkShell8, Color.FromArgb(159, 191, 96), "+08:00|006b|")
                    },
                    {
                        ChatType.TellToMe,
                        new ChatInfo(ChatType.TellToMe, Color.FromArgb(178, 129, 155), "+08:00|000d|")
                    },
                    {
                        ChatType.TellToOther,
                        new ChatInfo(ChatType.TellToOther, Color.FromArgb(178, 129, 155), "+08:00|000c|")
                    },
                    {
                        ChatType.Motion,
                        new ChatInfo(ChatType.Motion, Color.FromArgb(130, 178, 168), "+08:00|001d|")
                    },
                    {
                        ChatType.FreeCompany,
                        new ChatInfo(ChatType.FreeCompany, Color.FromArgb(134, 171, 178), "+08:00|0018|")
                    },
                    {
                        ChatType.Alliance,
                        new ChatInfo(ChatType.Alliance, Color.FromArgb(178, 89, 0), "+08:00|000f|")
                    }
                };
            }
        }
    }
}