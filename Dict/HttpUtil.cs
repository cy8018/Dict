using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dict
{
    class HttpUtil
    {
        private static string m_sApiUrl = "http://fanyi.youdao.com/openapi.do?keyfrom=MyDict-cy&key=2013312641&type=data&doctype=json&version=1.1&q=";

        public static string HttpRequest(string sInput)
        {
            string sResult = "";
            try
            {
                WebRequest wReq = System.Net.WebRequest.Create(m_sApiUrl + sInput);
                // Get the response instance.
                WebResponse wResp = wReq.GetResponse();

                System.IO.Stream respStream = wResp.GetResponseStream();

                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                {
                    sResult = reader.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                sResult = e.Message;
            }
            return sResult;
        }
    }
}
