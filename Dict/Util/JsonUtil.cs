using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dict.Model;

namespace Dict
{
    /// <summary>
    /// JSON string processing class
    /// </summary>
    public class JsonUtil
    {
        private static JsonUtil m_instance = null;

        public static JsonUtil GetInstance()
        {
            if (m_instance == null)
                m_instance = new JsonUtil();
            return m_instance;
        }

        public string BuildupStringResult(WordModel word)
        {
            string result = "";
            if (null != word)
            {
                if (word.IsSearchSuccessed)
                {
                    result = "[" + word.Phonetic + "]\r\n"
                        + word.Translation + "\r\n"
                        + word.Explains;
                }
                else
                {
                    result = word.Word;
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
        public WordModel ParseString(string str)
        {
            WordModel word = new WordModel();
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
                    word.IsSearchSuccessed = true;
                    word.Word = strQuery;
                    word.Phonetic = strPhonetic;
                    word.Translation = strTranslation;
                    word.Explains = strExplain;
                }
                else
                {
                    word.Word = strQuery;
                    word.IsSearchSuccessed = false;
                }
            }
            catch (Exception e)
            {
                word.IsSearchSuccessed = false;
                // Set the error message when exception
                word.ErrorMessage = "Error information:\r\n" + e.Message + "\r\n\r\nHttpResponse:\r\n" + str;
            }
            return word;
        }
    }
}
