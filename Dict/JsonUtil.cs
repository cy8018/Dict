using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dict
{
    class Word
    {
        public string word { get; set; }

        public string phonetic { get; set; }

        public string translation { get; set; }

        public string explains { get; set; }

        public bool isSearchSuccessed { get; set; }

        public string errorMessage { get; set; }
    }

    /// <summary>
    /// JSON string processing class
    /// </summary>
    class JsonUtil
    {
        private static JsonUtil m_instance = null;

        public static JsonUtil GetInstance()
        {
            if (m_instance == null)
                m_instance = new JsonUtil();
            return m_instance;
        }

        public string BuildupStringResult(Word word)
        {
            string result = "";
            if (null != word)
            {
                if (word.isSearchSuccessed)
                {
                    result = "[" + word.phonetic + "]\r\n"
                        + word.translation + "\r\n"
                        + word.explains;
                }
                else
                {
                    result = word.word;
                }
            }
            else
            {
                result = "Error: 'word == null'";
            }
            return result;
        }

        /// <summary>
        /// Parse the JSON string and build up the result
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Word ParseString(string str)
        {
            Word word = new Word();
            //string result = "";
            string strTranslation = "";
            string strPhonetic = "";
            string strExplain = "";
            string strQuery = "";
            try
            {
                // Parse the JSON result into object
                JObject data = (JObject)JsonConvert.DeserializeObject(str);
                // Get the searched word
                if (data["query"] != null)
                {
                    JValue query = (JValue)data["query"];
                    strQuery = query.ToString();
                }
                // Get the translation part
                if (data["translation"] != null)
                {
                    JArray translationArray = (JArray)data["translation"];
                    foreach (string translation in translationArray)
                    {
                        strTranslation = strTranslation + translation + "; ";
                    }
                }
                // Get the basic part
                if (data["basic"] != null)
                {
                    // phonetic
                    if (data["basic"]["phonetic"] != null)
                    {
                        JValue phonetic = (JValue)data["basic"]["phonetic"];
                        strPhonetic = /*"[" + */phonetic.ToString()/* + "]"*/;
                    }
                    // explains
                    if (data["basic"]["explains"] != null)
                    {
                        JArray explainsArray = (JArray)data["basic"]["explains"];
                        foreach (string explain in explainsArray)
                        {
                            strExplain = strExplain + explain + "\r\n";
                        }
                    }
                    word.isSearchSuccessed = true;
                    word.word = strQuery;
                    word.phonetic = strPhonetic;
                    word.translation = strTranslation;
                    word.explains = strExplain;
                }
                else
                {
                    word.word = strQuery;
                    word.isSearchSuccessed = false;
                }
            }
            catch (Exception e)
            {
                word.isSearchSuccessed = false;
                // Set the error message when exception
                word.errorMessage = "Error information:\r\n" + e.Message + "\r\n\r\nHttpResponse:\r\n" + str;
            }
            return word;
        }
    }
}
