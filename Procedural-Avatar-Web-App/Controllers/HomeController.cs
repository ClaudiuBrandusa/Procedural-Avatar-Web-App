using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Procedural_Avatar_Web_App.Models;
using Github_Like_Procedural_Avatar_API;
using System.Net.Http;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;

namespace Procedural_Avatar_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static string apiGetUrl = "https://procedural-avatar-api.azurewebsites.net/api/avatar/get";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["url"] = apiGetUrl;
            if(!GetConnectionStatus()) // if we can't connect to the API
            { // then we return the View
                return View();
            } // else we get an image
            ViewData["Img"] = apiGetUrl;
            return View();
        }

        [HttpGet]
        public string GetUrl(string color, int width, int height)
        { // returns the get key with headers
            if(color != null && color.Length > 3)
            {
                if(color[0] == '#')
                {
                    color = "%23"+color.Substring(1);
                    // 23% substitutes the '#' character in url headers
                }
            }
            return apiGetUrl + "?hexadecimalColor=" + color+"&width="+width+"&height="+height;
        }

        bool GetConnectionStatus()
        {
            // check if url exists
            try
            {
                WebRequest request = WebRequest.Create(apiGetUrl);

                WebResponse response = request.GetResponse();
                ViewData["Status"] = "1"; // status of API connection 
                return true;
                // then the url exists
            }
            catch (WebException ex)
            {
                // then the url does not exists
                // so we will just return the view
                ViewData["Status"] = "0";
                return false;
            }
        }

        [HttpGet]
        public IActionResult SetImage(string color, int width, int height)
        {
            return PartialView("SetImage", new UserData { apiUrl = GetUrl(color,width,height), status = GetConnectionStatus() });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
