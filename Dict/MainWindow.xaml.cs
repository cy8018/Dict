using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dict
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DoSearch()
        {
            string sResult = "";
            if (InputText.Text.Length > 0)
            {
                sResult = HttpUtil.HttpRequest(InputText.Text);
            }
            // parse the JSON string
            Result.Text = JsonUtil.GetInstance().ParseString(sResult);
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

        private void Button_Click_AddToWordBook(object sender, RoutedEventArgs e)
        {
            //DbUtil.GetInstance().OpenConnection();
            DbUtil.GetInstance().Select();
        }

        private void InputText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && InputText.Text.Length > 0)
            {
                DoSearch();
            }            
        }
    }
}
