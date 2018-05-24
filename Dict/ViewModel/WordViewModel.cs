using Dict.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dict.Model;
using MaterialDesignThemes.Wpf;

namespace Dict.ViewModel
{
    public class WordViewModel : ViewModelBase
    {
        #region Initialization and property definations
        public WordViewModel()
        {
            IsSyncWithCloudChecked = true;
            SearchWord = "test";
            Result = "";
            SearchCommand = new DelegateCommand(new Action<object>(SearchAsync));
            LoginCommand = new DelegateCommand(new Action<object>(Login));
            SnackbarMsgQ = new SnackbarMessageQueue();
        }

        public WordModel Word { get; set; }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
                RaisePropertyChanged("IsLoggedIn");
            }
        }

        private bool _isSyncWithCloudChecked;
        public bool IsSyncWithCloudChecked
        {
            get
            {
                return _isSyncWithCloudChecked;
            }
            set
            {
                _isSyncWithCloudChecked = value;
                RaisePropertyChanged("IsSyncWithCloudChecked");
            }
        }

        private string _searchWord;
        public string SearchWord
        {
            get
            {
                return _searchWord;
            }
            set
            {
                _searchWord = value;
                RaisePropertyChanged("SearchWord");
            }
        }

        private string _result;
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        private SnackbarMessageQueue _snackbarMsgQ;
        public SnackbarMessageQueue SnackbarMsgQ
        {
            get
            {
                return _snackbarMsgQ;
            }
            set
            {
                _snackbarMsgQ = value;
                RaisePropertyChanged("SnackbarMsgQ");
            }
        }

        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand LoginCommand { get; }

        #endregion

        private void Login(object obj)
        {

        }

        private async void SearchAsync(object obj)
        {
            Result = await DoSearchAsync(SearchWord);
            string msg = "";
            if (IsSyncWithCloudChecked)
            {
                if (await AddToDbAsync(Word))
                {
                    msg = "Syncronize with Cloud successfully.";
                }
                else
                {
                    msg = "Syncronize with Cloud failed.";
                }
                
                //the message queue can be called from any thread
                await Task.Factory.StartNew(() => SnackbarMsgQ.Enqueue(msg));
            }
        }

        private async Task<string> DoSearchAsync(string searchWord)
        {
            string sResult = "";
            string sInputText = searchWord.Trim();
            if (sInputText.Length > 0)
            {
                sResult = await Task.Run(()=>HttpUtil.HttpRequest(sInputText));
            }

            // parse the JSON string
            Word = JsonUtil.GetInstance().ParseString(sResult);

            // build up the result string and display it
            sResult = JsonUtil.GetInstance().BuildupStringResult(Word);

            return sResult;
        }

        private async Task<bool> AddToDbAsync(WordModel word)
        {
            if (word.IsSearchSuccessed && IsSyncWithCloudChecked)
            {
                // add the new word to the database
                return await DbUtil.GetInstance().AddNewWordAsync(word);
            }
            return false;
        }
    }
}
