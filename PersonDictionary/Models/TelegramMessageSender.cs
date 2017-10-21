using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

namespace PersonDictionary.Models
{
    public class TelegramMessageSender
    {
        // TODO: Replace the following with your gateway instance ID, 
        // Premium client ID and secret:
        private static string INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
        private static string CLIENT_ID = "FREE_TRIAL_ACCOUNT";
        private static string CLIENT_SECRET = "PUBLIC_SECRET";

        private static string API_URL = "http://api.whatsmate.net/v1/telegram/single/message/0" ;
       
        public bool sendMessage(string number, string message)
        {
            bool success = true;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                    client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                    Payload payloadObj = new Payload() { number = number, message = message };
                    string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                    client.Encoding = Encoding.UTF8;
                    string response = client.UploadString(API_URL, postData);
                    //Console.WriteLine(response);
                }
            }
            catch (WebException webEx)
            {
                //Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
                Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                String body = reader.ReadToEnd();
                //Console.WriteLine(body);
                success = false;
            }
            return success;
        }
    }
    public class Payload
    {
        public string number { get; set; }
        public string message { get; set; }
    }
}