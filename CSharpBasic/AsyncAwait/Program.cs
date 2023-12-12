using System.Net.Http;

namespace AsyncAwait
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Task<bool> uselessTask = SayHi5Times();
            uselessTask.Wait();
            Console.WriteLine($"{uselessTask.Result}");

            bool result = await SayHi5Times();
            Console.WriteLine(result);

            string url = "https://jsonplaceholder.typicode.com/todos/3";
            string content = await RequestHttpContentAsync(url);
            Console.WriteLine(content);
        }

        static async Task<string> RequestHttpContentAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }


        static async Task<bool> Dummy()
        {
            return await SayHi5Times();
        }

        static Task<bool> SayHi5Times()
        {
            return
                Task.Run(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine($"Hi {i} times");
                        Thread.Sleep(500);
                    }
                    return true;
                });
        }
    }
}