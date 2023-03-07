using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WeatherAPP1.Models;

namespace WeatherAPP1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Results()
        {
           
            return View();
        }

        [HttpPost]

      //  public async IActionResult WeatherAPIForm(Weather wm)
            public async Task<ActionResult> WeatherAPIForm(Weather wm) // porque é assyn temos k fazer isto


        {
            //HTTP Request CALL API
            var client = new HttpClient(); // nao sabemos que tipo é o client - var
            var request = new HttpRequestMessage(HttpMethod.Get, "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "?key=9ML3SDK9ZECE68356PEA4G45V");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // metodo para tratamento de exceçoes --> se da erro na pagina
            
            var body = await response.Content.ReadAsStringAsync(); // chamada assincrona e espera a minha resposta (response) e body recebe como string 
            dynamic weather = JsonConvert.DeserializeObject(body); // vai transformar dados API num array
            List<string> results = new List<string>();
            foreach (var day in weather.days)
            {
                results.Add("Forecast for date " + day.datetime);
                results.Add("General conditions will be: " + day.description);
                results.Add(" ");
            }
             ViewBag.output = results;
             return View("Results", wm); // recebe 2 argumentos // retorna a view

            ViewBag.Output = body;



            return View("Results");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}