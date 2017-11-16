using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Web.Script.Serialization;

namespace HttpClient
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Task t = new Task(HTTP_QUERIES);
            t.Start();
            Thread.Sleep(5000);
            Console.ReadLine();
        }
        static async void HTTP_QUERIES()
        {
            var TARGETURL = "http://localhost:49998";

            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://127.0.0.1:8888"),
                UseProxy = true,
            };

            // ... Use HttpClient.            
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient(handler);

            Console.WriteLine("Enter UserName");
            String userName = Console.ReadLine();

            Console.WriteLine("Enter Password");
            String password = Console.ReadLine();

            var byteArray = Encoding.ASCII.GetBytes(userName+':'+password);
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers
                .AuthenticationHeaderValue("Bearer", Convert.ToBase64String(byteArray));

            Console.WriteLine("To Get All");

            HttpResponseMessage response = await client.GetAsync(TARGETURL+"/api/Share");
            String content = await response.Content.ReadAsStringAsync();
            // ... Check Status Code    
            Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

            List<Test> _list = JsonConvert.DeserializeObject<List<Test>>(content);
            

            Console.WriteLine(content);
            // ... Read the strings.

            Console.WriteLine();
            foreach (var p in _list)
            {
                Console.WriteLine(p);
            }
            
            
            
            // ... Display the result.
            
            Console.ReadLine();
            Console.WriteLine("Choose your way.");
        }
        static void GetAll()
        {

        }
        static void DeleteById(Int32 Id)
        {

        }
        static void CreateNote()
        {

        } 
    }
    class Test
    {

        String test;

        String getTest() { return test; }
        void setTest(String test) { this.test = test; }

    }
}

