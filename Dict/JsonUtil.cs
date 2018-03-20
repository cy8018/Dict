using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dict
{
    class JsonUtil
    {
        private static JsonUtil m_instance = null;

        public static JsonUtil GetInstance()
        {
            if (m_instance == null)
                m_instance = new JsonUtil();
            return m_instance;
        }

        public string ParseString(string str)
        {
            string result = "";
            string strTranslation = "";
            string strPhonetic = "";
            string strExplain = "";
            JObject data = (JObject)JsonConvert.DeserializeObject(str);
            if(data["translation"] != null)
            {
                JArray translationArray = (JArray)data["translation"];
                foreach(string translation in translationArray)
                {
                    strTranslation = strTranslation + translation + "; ";
                }
            }

            if(data["basic"] != null)
            {
                if (data["basic"]["phonetic"] != null)
                {
                    JValue phonetic = (JValue)data["basic"]["phonetic"];
                    strPhonetic = "[" + phonetic.ToString() + "]\r\n";
                }
                if (data["basic"]["explains"] != null)
                {
                    JArray explainsArray = (JArray)data["basic"]["explains"];
                    foreach (string explain in explainsArray)
                    {
                        strExplain = strExplain + explain + "\r\n";
                    }
                }
            }

            result = strPhonetic + strTranslation + "\r\n" + strExplain;

            return result;
        }
    }
}
