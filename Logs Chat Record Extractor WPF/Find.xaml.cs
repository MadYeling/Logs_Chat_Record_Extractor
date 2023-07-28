using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Logs_Chat_Record_Extractor_WPF
{
    /// <summary>
    /// Find.xaml 的交互逻辑
    /// </summary>
    public partial class Find
    {
        /// <summary>
        /// 是否大小写
        /// </summary>
        private static bool _isMatchCase = true;

        /// <summary>
        /// 是否循环
        /// </summary>
        private static bool _isLoop;

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

        public Find(RichTextBox richTextBox)
        {
            InitializeComponent();
            _richTextBox = richTextBox;
            Init();
        }

        private void Init()
        {
            // 注册关闭事件
            Closing += OnClosing;
            // 先把按钮状态设置一下
            MatchCaseCheckBox.IsChecked = _isMatchCase;
            LoopCheckBox.IsChecked = _isLoop;
            UpRadioButton.IsChecked = _direction;
            DownRadioButton.IsChecked = !_direction;

            string clipboardText;

            // 三个优先级，最优先填充选择内容，其次上次搜索内容，最后剪切板内容
            // 都没有就把按钮禁掉
            if (!"".Equals(_richTextBox.Selection.Text))
            {
                ContentTextBox.Text = _richTextBox.Selection.Text;
            }
            else if (!"".Equals(_lastFindText))
            {
                ContentTextBox.Text = _lastFindText;
            }
            // 当剪切板里面的东西太长的时候，大概是复制到了奇怪的东西，就不填进去了
            else if (Clipboard.ContainsText(TextDataFormat.Text) &&
                     (clipboardText = Clipboard.GetText(TextDataFormat.Text)).Length < 10)
            {
                ContentTextBox.Text = clipboardText;
            }
            else
            {
                FindButton.IsEnabled = false;
            }
        }


        private void OnClosing(object sender, CancelEventArgs e)
        {
            _lastFindText = ContentTextBox.Text;
        }

        private void MatchCaseCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _isMatchCase = (bool)MatchCaseCheckBox.IsChecked;
        }

        private void MatchCaseCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _isMatchCase = (bool)MatchCaseCheckBox.IsChecked;
        }

        private void LoopCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _isLoop = (bool)LoopCheckBox.IsChecked;
        }

        private void LoopCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _isLoop = (bool)LoopCheckBox.IsChecked;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ContentTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            FindButton.IsEnabled = !"".Equals(ContentTextBox.Text);
        }

        
    }
}