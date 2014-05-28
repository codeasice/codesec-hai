using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Collections.Specialized;

namespace codesec
{
    class FayeClient
    {
        public String fayeURL;

        public FayeClient()
        {
            fayeURL = ConfigurationSettings.AppSettings["fayeURL"];
        }

        public void ZoneStatus(String key, String value)
        {
            if (value.Equals("0")) return;  // Only report when sensors become active 

            String data = "{\\\"zone\\\":\\\"" + key + "\\\", \\\"stamp\\\":\\\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK") + "\\\"}";
            String str = "{\"channel\":\"/messages/zonestatus\", \"data\":\"" + data + "\"}";

            using (System.Net.WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(fayeURL, new NameValueCollection()
                {
                   { "message",  str},
                });
            }
        }

        public void Message(String value)
        {
            String data = "{\\\"msg\\\":\\\"" + value + "\\\", \\\"stamp\\\":\\\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK") + "\\\"}";

            String str = "{\"channel\":\"/messages/new\", \"data\":\"" + data + "\"}";
            try
            {
                using (System.Net.WebClient client = new WebClient())
                {
                    byte[] response = client.UploadValues(fayeURL, new NameValueCollection()
                {
                   { "message",  str},
                });
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
