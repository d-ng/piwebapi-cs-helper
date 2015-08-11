using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using piwebapi_cs_helper;
using Newtonsoft.Json.Linq;

namespace piwebapi_cs_console_test
{
    class Program
    {
        /* Console application that makes GET request to a specified URL and display the response
         * as a string to the console. */
        static void Main(string[] args)
        {
            PIWebAPIClient piWebAPIClient = new PIWebAPIClient();
            do
            {
                try
                {
                    Console.Write("Enter URL: ");
                    string url = Console.ReadLine();
                    JObject jobj = piWebAPIClient.GetRequest(url);
                    Console.WriteLine(jobj.ToString());
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                finally
                {
                    Console.WriteLine("Press any key to continue (esc to exit)...");
                }

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
            piWebAPIClient.Dispose();
            Console.ReadKey();
        }
    }
}
