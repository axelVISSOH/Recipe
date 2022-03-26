using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using RecipeAPI.Models;
using Microsoft.AspNetCore.Http;

namespace RecipeWebClient.Controllers
{
    public class MyRecipesController : Controller
    {
        public IActionResult Index()
        {
            var actualConnectedUser = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("actualConnectedUser"));
            return View(actualConnectedUser);
        }
    }
}
