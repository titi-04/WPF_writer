using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF_4
{
    /// <summary>
    /// MyDocumentViewer.xaml 的互動邏輯
    /// </summary>
    public partial class MyDocumentViewer : Window
    {
        Color fontColor = Colors.Black;//字型顏色 預設黑色
        Color backgroundColor = Colors.LightSkyBlue;//背景顏色 預設白色
        public MyDocumentViewer()
        {
            InitializeComponent();
            fontColorPicker.SelectedColor = fontColor;//先把字型填滿顏色
            backgroundColorPicker.SelectedColor = backgroundColor;//先把背景填滿顏色

            //將系統中的每一個字型(FontFamily)加入
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                fontFamilyComboBox.Items.Add(fontFamily.Source);
            }
            fontFamilyComboBox.SelectedItem = "DFPBiaoKaiShu-B5";//預設字型
            //或者fontFamilyComboBox.SelectedIndex = 10;

            fontSizeComboBox.ItemsSource = new List<double>() { 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 32, 40, 50, 60, 80, 100 };
            fontSizeComboBox.SelectedIndex = 3;//預設值

        }

        private void NewCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            MyDocumentViewer myDocumentViewer = new MyDocumentViewer();
            myDocumentViewer.Show();
        }

        private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //開啟檔案
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*";
            openFileDialog.DefaultExt = ".rtf";//預設副檔名
            openFileDialog.AddExtension = true;//自動加副檔名

            //如果有打開檔案
            if (openFileDialog.ShowDialog() == true)
            {
                //讀取檔案
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);//開啟檔案
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);//範圍是從開始到結束
                range.Load(fileStream, DataFormats.Rtf);//讀取範圍
                fileStream.Close();//關閉檔案
            }
        }

        private void SaveCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //儲存檔案
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|HTML (*.html)|*.html|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = ".rtf";//預設副檔名
            saveFileDialog.AddExtension = true;//自動加副檔名

            //如果有打開檔案
            if (saveFileDialog.ShowDialog() == true)
            {
                //取得範圍
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);//範圍是從開始到結束
                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);//建立檔案
                range.Save(fileStream, DataFormats.Rtf);//儲存範圍
                fileStream.Close();//關閉檔案
            }
        }

        private void fontColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fontColor = (Color)e.NewValue;//改變後得新的值
            SolidColorBrush fontBrush = new SolidColorBrush(fontColor);//用顏色產生筆刷，並將顏色運用到所選文字
            rtbEditor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, fontBrush);
            //將rtbEditor.所選擇的文字.應用這個文字(前景)的屬性.設定為fontBrush
        }

        private void backgroundColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            backgroundColor = (Color)e.NewValue;//改變後得新的值
            SolidColorBrush backgroundBrush = new SolidColorBrush(backgroundColor);//用顏色產生筆刷，並將顏色運用到所選文字
            rtbEditor.Background = backgroundBrush;//將rtbEditor的背景設定為backgroundBrush
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
                //將rtbEditor.所選擇的文字.應用這個文字(字型)的屬性.設定為fontFamilyComboBox.SelectedItem
            }
        }

        private void fontSizeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontSizeComboBox.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSizeComboBox.SelectedItem);
                //將rtbEditor.所選擇的文字.應用這個文字(字型大小)的屬性.設定為fontSizeComboBox.SelectedItem
            }
        }

        //將每一個不同的property同步反應到相對應的UI元件
        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //var可自動判斷型別 //Selection是選取的範圍 //GetPropertyValue取得屬性值
            var property_bold = rtbEditor.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            boldButton.IsChecked = (property_bold != DependencyProperty.UnsetValue) && (property_bold.Equals(FontWeights.Bold));
            //有按下去稱IsChecked //不是預設值稱UnsetValue 所以!=UnsetValue就是有設定值 //判斷是否有粗體

            var property_italic = rtbEditor.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            italicButton.IsChecked = (property_italic != DependencyProperty.UnsetValue) && (property_italic.Equals(FontStyles.Italic));
            //判斷是否有斜體

            var property_underline = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underlineButton.IsChecked = (property_underline != DependencyProperty.UnsetValue) && (property_underline.Equals(TextDecorations.Underline));
            //判斷是否有底線

            var property_fontFamily = rtbEditor.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            fontFamilyComboBox.SelectedItem = property_fontFamily.ToString();

            var property_fontSize = rtbEditor.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            fontSizeComboBox.SelectedItem = property_fontSize;
            
            var property_fontColor = rtbEditor.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            fontColorPicker.SelectedColor = ((SolidColorBrush)property_fontColor).Color;
        }

        private void trashButton_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.Document.Blocks.Clear();//清空文件
        }
    }
}
