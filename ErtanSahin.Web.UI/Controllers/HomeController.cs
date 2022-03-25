using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErtanSahin.Web.UI.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace ErtanSahin.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration _configuration;


        readonly HttpClientHandler _clientHandler = new HttpClientHandler();
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientHandler.ServerCertificateCustomValidationCallback = (csender, cert, chain, sslPolicyErrors) => true;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ReservationViewModel model = new ReservationViewModel();

            var client = new HttpClient(_clientHandler);
            client.DefaultRequestHeaders.Add("api-key", "ACECA62D-F4A9-4973-91E7-140043816A72");
            var response = client.GetAsync(new Uri($"{_configuration.GetValue<string>("api-path")}api/reservation/rooms")).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<List<RoomModel>>(content);

                model.Rooms = responseModel;
            }


            return View(model);
        }
        public IActionResult Create(ReservationViewModel model)
        {
            var mails = model.CreateForm.Mails.Split(";");
            foreach (var item in mails)
            {
                model.CreateForm.Members.Add(new CreateForm.Member()
                {
                    Email = item//TODO Valid to mail
                });
            }

            var postString = JsonConvert.SerializeObject(model.CreateForm);


            var client = new HttpClient(_clientHandler);
            client.DefaultRequestHeaders.Add("api-key", "ACECA62D-F4A9-4973-91E7-140043816A72");
            var response = client.PostAsync(new Uri($"{_configuration.GetValue<string>("api-path")}api/reservation/create"), new StringContent(postString, Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Completed");
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction("Error", new { message = content });
            }

        }
        public IActionResult Completed()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
