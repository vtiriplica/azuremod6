using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace devops_windows.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new IndexModel
            {
                Host = LocalIPAddress().ToString(),
                OS = Environment.OSVersion.ToString(),
                Framework = Environment.Version.ToString(),
                Quotes = await GetQuotes()
            };

            return View(model);
        }

        private async Task<string[]> GetQuotes()
        {
            string[] quotes = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://demowebapi:9000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                try
                {
                    HttpResponseMessage response = await client.GetAsync("/api/quotes");
                    if (response.IsSuccessStatusCode)
                    {
                        quotes = await response.Content.ReadAsAsync<string[]>();

                    } else
                    {
                        quotes = new[] {$"Internal Server Error- {response.StatusCode}:{response.ReasonPhrase}" };
                    }
                }
                catch (Exception ex)
                {
                    quotes = new[] { "FAILED to contact Quotes Service" };
                }
            }

        
            return quotes;
        }

        private IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}