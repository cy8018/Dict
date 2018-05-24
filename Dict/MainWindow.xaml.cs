using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Dict.Model;

namespace Dict
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DoSearchAsync()
        {
            string sResult = "";
            string sInputText = InputText.Text.Trim();
            if (sInputText.Length > 0)
            {
                sResult = HttpUtil.HttpRequest(sInputText);
            }

            // parse the JSON string
            WordModel newWord = JsonUtil.GetInstance().ParseString(sResult);

            // build up the result string and display it
            Result.Text = JsonUtil.GetInstance().BuildupStringResult(newWord);

            if (newWord.IsSearchSuccessed)
            {
                // add the new word to the database
                if(await DbUtil.GetInstance().AddNewWordAsync(newWord))
                {
                    Result.Text += "\nSync with Cloud successfully.";
                }
                else
                {
                    Result.Text += "\nSync with Cloud failed.";
                }
            }

        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            DoSearchAsync();
        }

        private void Button_Click_AddToWordBook(object sender, RoutedEventArgs e)
        {
            //DbUtil.GetInstance().OpenConnection();
            //DbUtil.GetInstance().Select();
        }

        private void InputText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && InputText.Text.Length > 0)
            {
                DoSearchAsync();
            }            
        }
    }
}
