using Microsoft.AspNetCore.Mvc;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //Get All Regiongs from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7076/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

               var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();

                ViewBag.Response = stringResponseBody;
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }
    }
}
