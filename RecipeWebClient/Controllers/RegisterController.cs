using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeAPI.Models;
using RecipeWebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RecipeWebClient.Controllers
{
    public class RegisterController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private LoginController lc = new LoginController();
        public IActionResult Register()
        {
           return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            user.Status = "user";           
            if (ModelState.IsValid)
            {
                if(lc.GetPerson(user.Mail, user.Password) == null)
                {
                    var URI = AddUsserAsync(user);
                    UserViewModel viewModel = new UserViewModel { Person = user, isConnected = false };
                    return Redirect("/Login");
                }               
            }
            ModelState.AddModelError("Person.Mail", "mail et/ou mot de passe incorrect(s)");
            return View(user);
        }

        public async Task<Uri> AddUsserAsync(User user)
        {            
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:3305/api/users/", user);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;            
        }
    }
}