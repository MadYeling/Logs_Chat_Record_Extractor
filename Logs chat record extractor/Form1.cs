using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Logs_chat_record_extractor.Models;

namespace Logs_chat_record_extractor
{
    public partial class FormOpen : Form
    {
        /// <summary>
        /// 选择文件对话框
        /// </summary>
        private OpenFileDialog _openFileDialog1;

        /// <summary>
        /// 显示聊天内容的主窗口对象
        /// </summary>
        private MainForm _mf;

        /// <summary>
        /// 聊天记录数组
        /// </summary>
        private readonly ArrayList _chatList;

        /// <summary>
        /// 聊天记录信息数组
        /// </summary>
        private static ArrayList _chatInfoList;

        /// <summary>
        /// 主窗口标题
        /// </summary>
        private string _mainFormTitle;

        /// <summary>
        /// 构造器
        /// </summary>
        public FormOpen()
        {
            InitializeComponent();
            InitChecked();
            LoadChatTable();
            _chatList = new ArrayList();
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 选择文件 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (_openFileDialog1.ShowDialog() != DialogResult.OK) return;
            textBox1.Text = Path.GetFullPath(_openFileDialog1.FileName);
            _mainFormTitle = Path.GetFileName(_openFileDialog1.FileName);
        }

        /// <summary>
        /// 加载 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            _chatList.Clear();
            using (
                var sr = new StreamReader(
                    new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                )
            )
            {
                string line;
                // 读取文件
                while ((line = sr.ReadLine()) != null)
                {
                    var chatInfo = IsAChatMessage(line);
                    if (chatInfo != null)
                    {
                        // 装载内容
                        _chatList.Add(LoadChatsBean(chatInfo, line));
                    }
                }
            }

            _mf = new MainForm(_chatList, _mainFormTitle);
            _mf.Show(this);
            Hide();
            label1.Text = "就绪";
        }

        /// <summary>
        /// 初始化过滤器
        /// </summary>
        private static void InitChecked()
        {
            for (var i = 0; i < ChatTypeHandler.GetIsShowArrayLength(); i++)
            {
                if (i == (int) ChatType.Party || i == (int) ChatType.Speak ||
                    i == (int) ChatType.Yell || i == (int) ChatType.Alliance ||
                    i == (int) ChatType.Shout || i == (int) ChatType.Motion ||
                    i == (int) ChatType.Tell)
                {
                    ChatTypeHandler.SetIsShowThisChatType(i, true);
                }
            }
        }

        /// <summary>
        /// 校验读取的目标是否为聊天内容
        /// 是则返回聊天信息对象，不是则返回null
        /// </summary>
        /// <param name="line">需要校验的内容</param>
        /// <returns>返回对象</returns>
        private static ChatInfo IsAChatMessage(string line)
        {
            return _chatInfoList.Cast<ChatInfo>().FirstOrDefault(chatInfo => line.Contains(chatInfo.ChatCode));
        }

        /// <summary>
        /// 装载聊天信息
        /// </summary>
        /// <param name="chatInfo">聊天信息详情</param>
        /// <param name="line">内容</param>
        /// <returns>装载完成的聊天信息</returns>
        private static Chat LoadChatsBean(ChatInfo chatInfo, string line)
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
                .Replace("", "Ⓒ");
            var sp = line.Split('|');
            var time = sp[1].Substring(11, 5);
            var chatsBean = new Chat
            {
                Show = ChatTypeHandler.IsShowThisChatType(chatInfo.ChatType),
                ChatInfo = chatInfo,
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
            _chatInfoList = new ArrayList
            {
                new ChatInfo(ChatType.Speak, Color.FromArgb(178, 178, 178), "+08:00|000a|"),
                new ChatInfo(ChatType.Yell, Color.FromArgb(178, 178, 0), "+08:00|001e|"),
                new ChatInfo(ChatType.Shout, Color.FromArgb(178, 116, 71), "+08:00|000b|"),
                new ChatInfo(ChatType.Party, Color.FromArgb(71, 168, 178), "+08:00|000e|"),
                new ChatInfo(ChatType.CwLinkShell1, Color.FromArgb(159, 191, 96), "+08:00|0025|"),
                new ChatInfo(ChatType.CwLinkShell2, Color.FromArgb(159, 191, 96), "+08:00|0065|"),
                new ChatInfo(ChatType.CwLinkShell3, Color.FromArgb(159, 191, 96), "+08:00|0066|"),
                new ChatInfo(ChatType.CwLinkShell4, Color.FromArgb(159, 191, 96), "+08:00|0067|"),
                new ChatInfo(ChatType.CwLinkShell5, Color.FromArgb(159, 191, 96), "+08:00|0068|"),
                new ChatInfo(ChatType.CwLinkShell6, Color.FromArgb(159, 191, 96), "+08:00|0069|"),
                new ChatInfo(ChatType.CwLinkShell7, Color.FromArgb(159, 191, 96), "+08:00|006a|"),
                new ChatInfo(ChatType.CwLinkShell8, Color.FromArgb(159, 191, 96), "+08:00|006b|"),
                new ChatInfo(ChatType.LinkShell1, Color.FromArgb(159, 191, 96), "+08:00|0010|"),
                new ChatInfo(ChatType.LinkShell2, Color.FromArgb(159, 191, 96), "+08:00|0011|"),
                new ChatInfo(ChatType.LinkShell3, Color.FromArgb(159, 191, 96), "+08:00|0012|"),
                new ChatInfo(ChatType.LinkShell4, Color.FromArgb(159, 191, 96), "+08:00|0013|"),
                new ChatInfo(ChatType.LinkShell5, Color.FromArgb(159, 191, 96), "+08:00|0014|"),
                new ChatInfo(ChatType.LinkShell6, Color.FromArgb(159, 191, 96), "+08:00|0015|"),
                new ChatInfo(ChatType.LinkShell7, Color.FromArgb(159, 191, 96), "+08:00|0016|"),
                new ChatInfo(ChatType.LinkShell8, Color.FromArgb(159, 191, 96), "+08:00|0017|"),
                new ChatInfo(ChatType.Beginner, Color.FromArgb(159, 191, 96), "+08:00|001b|"),
                new ChatInfo(ChatType.Tell, Color.FromArgb(178, 129, 155), "+08:00|000d|"),
                new ChatInfo(ChatType.TellToOther, Color.FromArgb(178, 129, 155), "+08:00|000c|"),
                //官方情感动作
                new ChatInfo(ChatType.Motion, Color.FromArgb(130, 178, 168), "+08:00|001d|"),
                //自定义的情感动作
                new ChatInfo(ChatType.MotionCustom, Color.FromArgb(130, 178, 168), "+08:00|001c|"),
                new ChatInfo(ChatType.FreeCompany, Color.FromArgb(134, 171, 178), "+08:00|0018|"),
                new ChatInfo(ChatType.Alliance, Color.FromArgb(178, 89, 0), "+08:00|000f|"),
            };
        }
    }
}