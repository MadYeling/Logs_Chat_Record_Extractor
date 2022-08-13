using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
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
        /// 聊天记录集合
        /// </summary>
        private readonly List<Chat> _chatList;

        /// <summary>
        /// 聊天记录信息集合
        /// </summary>
        private static List<ChatInfo> _chatInfoList;

        /// <summary>
        /// 主窗口标题
        /// </summary>
        private string _mainFormTitle;

        /// <summary>
        /// 时区，用于筛选
        /// </summary>
        private static string _timeZone = "+08:00";

        /// <summary>
        /// 构造器
        /// </summary>
        public FormOpen()
        {
            InitializeComponent();
            InitChecked();
            InitChatInfoList();
            _chatList = new List<Chat>();
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
            var lineNub = 1;
            using (
                var sr = new StreamReader(
                    new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                )
            )
            {
                string line;

                // 获取时区
                line = sr.ReadLine();
                var tempZoneStr = line.Split('|')[1];
                _timeZone = tempZoneStr.Substring(tempZoneStr.Length - 6);
                Console.WriteLine(_timeZone);

                // 读取文件
                do
                {
                    if (line.Split('|')[0] == "01")
                    {
                        lineNub = 1;
                    }

                    var chatInfo = IsAChatMessage(line);
                    if (chatInfo != null)
                    {
                        // 装载内容
                        _chatList.Add(LoadChat(chatInfo, line, lineNub));
                    }

                    lineNub++;
                } while ((line = sr.ReadLine()) != null);
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
                if (i == (int)ChatType.Party || i == (int)ChatType.Speak ||
                    i == (int)ChatType.Yell || i == (int)ChatType.Alliance ||
                    i == (int)ChatType.Shout || i == (int)ChatType.Motion ||
                    i == (int)ChatType.Tell)
                {
                    ChatTypeHandler.SetIsShowThisChatType(i, true);
                }
            }
        }

        /// <summary>
        /// 校验读取的目标是否为聊天内容
        /// 是则返回聊天信息对象，不是则返回null
        /// 更新对6.0log支持，同时支持5.0log
        /// </summary>
        /// <param name="line">需要校验的内容</param>
        /// <returns>返回对象</returns>
        private static ChatInfo IsAChatMessage(string line)
        {
            return _chatInfoList.FirstOrDefault(chatInfo =>
                line.Contains(_timeZone + chatInfo.ChatCode) || line.Contains(_timeZone + chatInfo.ChatCode.ToUpper()));
        }

        /// <summary>
        /// 装载聊天信息
        /// </summary>
        /// <param name="chatInfo">聊天信息详情</param>
        /// <param name="line">内容</param>
        /// <returns>装载完成的聊天信息</returns>
        private static Chat LoadChat(ChatInfo chatInfo, string line, int lineNub)
        {
            var ck = getAndRemoveCheck(line);
            var mck = TextChecker.Encrypt(ck.lineText, lineNub);
            if (ck.lineCheck != mck)
            {
                MessageBox.Show(
                    $"警告：存在被篡改文本。\r\n文本内容：{ck.lineText}\r\n软件校验结果：{mck}\r\n文中校验码：{ck.lineCheck}");
            }

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
            var chat = new Chat
            {
                Show = ChatTypeHandler.IsShowThisChatType(chatInfo.ChatType),
                ChatInfo = chatInfo,
                Time = time,
                PlayerName = sp[3],
                Context = sp[4]
            };
            return chat;
        }

        private static Line getAndRemoveCheck(string line)
        {
            var l = line.Split('|');
            var check = l[l.Length - 1];
            var d = new List<string>();
            for (var i = 0; i < l.Length - 1; i++)
            {
                d.Add(l[i]);
            }

            var res = string.Join("|", d);
            Console.WriteLine(res);
            return new Line(res, check);
        }

        /// <summary>
        /// 初始化聊天信息集合
        /// </summary>
        private static void InitChatInfoList()
        {
            _chatInfoList = new List<ChatInfo>
            {
                new ChatInfo(ChatType.Speak, Color.FromArgb(178, 178, 178), "|000a|"),
                new ChatInfo(ChatType.Yell, Color.FromArgb(178, 178, 0), "|001e|"),
                new ChatInfo(ChatType.Shout, Color.FromArgb(178, 116, 71), "|000b|"),
                new ChatInfo(ChatType.Party, Color.FromArgb(71, 168, 178), "|000e|"),
                new ChatInfo(ChatType.CwLinkShell1, Color.FromArgb(159, 191, 96), "|0025|"),
                new ChatInfo(ChatType.CwLinkShell2, Color.FromArgb(159, 191, 96), "|0065|"),
                new ChatInfo(ChatType.CwLinkShell3, Color.FromArgb(159, 191, 96), "|0066|"),
                new ChatInfo(ChatType.CwLinkShell4, Color.FromArgb(159, 191, 96), "|0067|"),
                new ChatInfo(ChatType.CwLinkShell5, Color.FromArgb(159, 191, 96), "|0068|"),
                new ChatInfo(ChatType.CwLinkShell6, Color.FromArgb(159, 191, 96), "|0069|"),
                new ChatInfo(ChatType.CwLinkShell7, Color.FromArgb(159, 191, 96), "|006a|"),
                new ChatInfo(ChatType.CwLinkShell8, Color.FromArgb(159, 191, 96), "|006b|"),
                new ChatInfo(ChatType.LinkShell1, Color.FromArgb(159, 191, 96), "|0010|"),
                new ChatInfo(ChatType.LinkShell2, Color.FromArgb(159, 191, 96), "|0011|"),
                new ChatInfo(ChatType.LinkShell3, Color.FromArgb(159, 191, 96), "|0012|"),
                new ChatInfo(ChatType.LinkShell4, Color.FromArgb(159, 191, 96), "|0013|"),
                new ChatInfo(ChatType.LinkShell5, Color.FromArgb(159, 191, 96), "|0014|"),
                new ChatInfo(ChatType.LinkShell6, Color.FromArgb(159, 191, 96), "|0015|"),
                new ChatInfo(ChatType.LinkShell7, Color.FromArgb(159, 191, 96), "|0016|"),
                new ChatInfo(ChatType.LinkShell8, Color.FromArgb(159, 191, 96), "|0017|"),
                new ChatInfo(ChatType.Beginner, Color.FromArgb(159, 191, 96), "|001b|"),
                new ChatInfo(ChatType.Tell, Color.FromArgb(178, 129, 155), "|000d|"),
                new ChatInfo(ChatType.TellToOther, Color.FromArgb(178, 129, 155), "|000c|"),
                //官方情感动作
                new ChatInfo(ChatType.Motion, Color.FromArgb(130, 178, 168), "|001d|"),
                //自定义的情感动作
                new ChatInfo(ChatType.MotionCustom, Color.FromArgb(130, 178, 168), "|001c|"),
                new ChatInfo(ChatType.FreeCompany, Color.FromArgb(134, 171, 178), "|0018|"),
                new ChatInfo(ChatType.Alliance, Color.FromArgb(178, 89, 0), "|000f|"),
            };
        }
    }
}