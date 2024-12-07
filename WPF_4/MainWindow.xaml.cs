using System.Windows;

namespace WPF_4
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Click觸發一個事件
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //呼叫主視窗
            MyDocumentViewer myDocumentViewer = new MyDocumentViewer();
            myDocumentViewer.Show();//Show有從屬關係(可開多個檔案) ShowDialog無從屬關係(關閉檔案才能再開新檔案)
        }
    }
}
