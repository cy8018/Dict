using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dict.Model
{
    public class WordModel
    {
        public string SearchWord { get; set; }

        public string Word { get ; set; }

        public string Phonetic { get; set; }

        public string Translation { get; set; }

        public string Explains { get; set; }

        public bool IsSearchSuccessed { get; set; }

        public string ErrorMessage { get; set; }
    }
}
