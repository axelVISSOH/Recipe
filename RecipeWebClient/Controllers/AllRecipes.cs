using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeWebClient.Controllers
{
    public class AllRecipes : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            List<Recipe> recipes = (List<Recipe>)GetPublicRecipes((List<Recipe>)GetRecipes().Result);
            List<User> users = (List<User>)GetUsersOfPublicRecipes((List<Recipe>)GetRecipes().Result);
            ViewBag.publicRecipes   = recipes;
            ViewBag.associatedUsers = users;
            return View(recipes);
        }

        public async Task<ICollection<Recipe>> GetRecipes()
        {
            ICollection<Recipe> Recipes = new List<Recipe>();
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/recipes/").Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                Recipes = JsonConvert.DeserializeObject<List<Recipe>>(resp);
            }
            return Recipes;
        }

        public ICollection<Recipe> GetPublicRecipes(List<Recipe> recipes)
        {
            ICollection<Recipe> publicRecipes = new List<Recipe>();

            foreach (Recipe r in recipes)
            {
                if (r.IsPublic)
                    publicRecipes.Add(r);
            }
            return publicRecipes;
        }

        public ICollection<User> GetUsersOfPublicRecipes(List<Recipe> recipes)
        {
            ICollection<User> users = new List<User>();

            foreach (Recipe r in recipes)
            {
                users.Add(GetUser(r.UserID).Result);           
            }

            return users;
        }

        public async Task<User> GetUser(int? id)
        {
            User user = null;
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/users/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(resp);
            }
            return user;
        }
    }
}
