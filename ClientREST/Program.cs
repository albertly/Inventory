using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientREST
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            var baseURL = "https://localhost:5002/";
            client.BaseAddress = new Uri(baseURL);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var policy = Policy.Handle<HttpRequestException>()                    
                    .WaitAndRetry(4,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         (exception, timeSpan, retryCount, context) =>
                             {
                                 Console.WriteLine("timeSpan:" + timeSpan);
                                 Console.WriteLine("retryCount:" + retryCount);
                             });

            try
            {

                var data = policy.Execute<string>(() => MakeCall(client).GetAwaiter().GetResult());                

                Console.WriteLine(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Hello World!");
        }


        static async Task<string> MakeCall(HttpClient client)
        {
            string returnData = "";
            try
            {
                var result = await client.GetAsync("/api/User/User?Id=45");

                var data = await result.Content.ReadAsStringAsync();

                returnData = data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return returnData;
        }
    }
}
