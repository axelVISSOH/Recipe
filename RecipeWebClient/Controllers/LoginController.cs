using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeAPI.Models;
using RecipeWebClient.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeWebClient.Controllers
{
    public class LoginController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public IActionResult Login()
        {
            UserViewModel viewModel = new UserViewModel { isConnected = false };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(UserViewModel viewModel)
        {
            Person personToConnect = GetPerson(viewModel.Person.Mail, viewModel.Person.Password).Result;
            if (personToConnect != null)
            {
                HttpContext.Session.SetString("actualConnectedUser", JsonConvert.SerializeObject(personToConnect));
                
                return personToConnect.Status.Equals("user")? Redirect("/MyRecipes/Index") : Redirect("/Admin/Index");
            }
            ModelState.AddModelError("Person.Mail", "mail et/ou mot de passe incorrect(s)");
            return View(viewModel);
        }

        public async Task<Person> GetPerson(string mail, string password)
        {
            Person personToConnect = null;
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/people/" + mail + "/" + password).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                personToConnect = JsonConvert.DeserializeObject<Person>(resp);
            }
            return personToConnect;
        }

        public ActionResult LogOff()
        {
            HttpContext.Session.Remove("actualConnectedUser");
            return Redirect("/");
        }
    }
}
