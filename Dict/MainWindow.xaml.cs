﻿using System;
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
        
        private Word DoSearch()
        {
            string sResult = "";
            string sInputText = InputText.Text.Trim();
            if (sInputText.Length > 0)
            {
                sResult = HttpUtil.HttpRequest(sInputText);
            }

            // parse the JSON string
            Word newWord = JsonUtil.GetInstance().ParseString(sResult);

            // build up the result string and display it
            Result.Text = JsonUtil.GetInstance().BuildupStringResult(newWord);

            return newWord;
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            // Only do the search when click the button, not add to database
            DoSearch();
        }

        private void InputText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && InputText.Text.Length > 0)
            {
                Word newWord = DoSearch();
                if (newWord.isSearchSuccessed)
                {
                    // add the new word to the database
                    DbUtil.GetInstance().AsyncAddNewWord(newWord);
                }
            }            
        }
    }
}
