using System.ComponentModel;
using System.Windows;
using System.Windows.Shapes;
using HandyControl.Tools.Extension;
using Logs_Chat_Record_Extractor_WPF.Models;
using Microsoft.Win32;

namespace Logs_Chat_Record_Extractor_WPF
{
    /// <summary>
    /// FileChooser.xaml 的交互逻辑
    /// </summary>
    public partial class FileChooser
    {
        private OpenFileDialog _openFileDialog;
        private string _logFilePath = "";
        private MainWindow _mainWindow;

        public FileChooser()
        {
            InitializeComponent();
            Init();
            InitChecked();
        }

        private void Init()
        {
            Closing += Window_Closing;
            _openFileDialog = new OpenFileDialog
            {
                Title = "请选择LOG文件",
                Filter = "log文件（*.log）|*.log"
            };
            _mainWindow = new MainWindow(this);
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

        private void Choose_OnClick(object sender, RoutedEventArgs e)
        {
            if (_openFileDialog.ShowDialog() != true) return;
            filePath.Text = _openFileDialog.FileName;
            _logFilePath = _openFileDialog.FileName;
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            _mainWindow.LoadFile(_logFilePath);
            _mainWindow.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _mainWindow.Killed();
            Application.Current.Shutdown(0);
        }
    }
}