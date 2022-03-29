using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RecipeWebClient.Controllers
{
    public class AdminController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllUsers()
        {
            ICollection<User> users = GetUsersAsync().Result;
            return View("AllUsers",users);
        }
        public IActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return null;
            }
            User user = GetUser(id).Result;
            return user == null ? null: View("DeleteUser",user);
        }        

        [HttpPost]
        public IActionResult DeleteUserConfirmed(int id)
        {
            var suppr = SupprUser(id);
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Comment comment = GetComment(id).Result;
            return comment == null ? null: View("DeleteComment", comment);
        }

        [HttpPost]
        public IActionResult DeleteCommentConfirmed(int id)
        {
            var suppr = SupprComment(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ValidateComment(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Comment comment = GetComment(id).Result;
            return comment == null ? null : View("ValidateComment", comment);
        }

        [HttpPost]
        public IActionResult ValidateCommentConfirmed(int id)
        {
            Comment commentToValidate = GetComment(id).Result;
            commentToValidate.IsReportable = false;
            commentToValidate.IsReport = false;
            var mod = ModifCommentAsync(commentToValidate);
            return mod == null ? null: RedirectToAction(nameof(Index));
        }

        public IActionResult ReportedComments()
        {
            List<Comment> reportedComments = (List<Comment>)GetReportedComments();
            List<string> userPseudos = (List<string>)GetReportersPseudo(reportedComments);
            List<Recipe> reportedRecipes = (List<Recipe>)GetReportedRecipes(reportedComments);
            ViewBag.reportedComments = reportedComments;
            ViewBag.userPseudos      = userPseudos;
            ViewBag.reportedRecipes  = reportedRecipes;
            ViewBag.Message = "ReportedComments";
            return View();
        }

        public IActionResult UserComments(int? id)
        {
            if (id == null)
            {
                return null;
            }
            List<Comment> userComments = (List<Comment>)GetUserComments(id);
            List<string> userPseudos = (List<string>)GetReportersPseudo(userComments);
            List<Recipe> reportedRecipes = (List<Recipe>)GetReportedRecipes(userComments);

            ViewBag.reportedComments = userComments;
            ViewBag.userPseudos = userPseudos;
            ViewBag.reportedRecipes = reportedRecipes;
            ViewBag.Message = "UserComments";
            return View("ReportedComments");
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            ICollection<User> Users = new List<User>();
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/users/").Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                Users = JsonConvert.DeserializeObject<List<User>>(resp);
            }
            return Users;            
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

        public async Task<Recipe> GetRecipe(int? id)
        {
            Recipe recipe = null;
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/recipes/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                recipe = JsonConvert.DeserializeObject<Recipe>(resp);
            }
            return recipe;
        }

        public async Task<Comment> GetComment(int? id)
        {
            Comment comment = null;
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/comments/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                comment = JsonConvert.DeserializeObject<Comment>(resp);
            }
            return comment;
        }

        public async Task<Uri> SupprUser(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:3305/api/users/" + id);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        
        public async Task<Uri> SupprComment(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:3305/api/comments/" + id);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<ICollection<Comment>> GetCommentsAsync()
        {
            ICollection<Comment> Comments = new List<Comment>();
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/comments/").Result;
            if (response.IsSuccessStatusCode)
            { 
                var resp = await response.Content.ReadAsStringAsync();
                Comments = JsonConvert.DeserializeObject<List<Comment>>(resp);
            }
            return Comments;
        }

        public async Task<ICollection<Recipe>> GetRecipesAsync()
        {
            ICollection<Recipe> recipes = new List<Recipe>();
            HttpResponseMessage response = client.GetAsync("http://localhost:3305/api/recipes/").Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                recipes = JsonConvert.DeserializeObject<List<Recipe>>(resp);
            }
            return recipes;
        }

        public async Task<Uri> ModifCommentAsync(Comment comment)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("http://localhost:3305/api/comments/" + comment.Id, comment);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public ICollection<Comment> GetReportedComments()
        {
            ICollection<Comment> Comments = GetCommentsAsync().Result;
            ICollection<Comment> ReportedComments = new List<Comment>();
            
            foreach( Comment c in Comments)
            {
                if (c.IsReport)
                    ReportedComments.Add(c);
                else
                    continue;
            }            
            return ReportedComments;
        }

        public ICollection<String> GetReportersPseudo(List<Comment> comments)
        {
            ICollection<String> userPseudos = new List<String>();

            foreach (Comment c in comments)
            {
                userPseudos.Add(GetUser(c.UserID).Result.Pseudo);                
            }
            return userPseudos;
        }

        public ICollection<Recipe> GetReportedRecipes(List<Comment> comments)
        {
            ICollection<Recipe> recipe = new List<Recipe>();

            foreach (Comment c in comments)
            {
                recipe.Add(GetRecipe(c.RecipeID).Result);
            }
            return recipe;
        }

        public ICollection<Comment> GetUserComments(int? userId)
        {
            ICollection<Comment> userComments = new List<Comment>();

            foreach (Comment c in GetCommentsAsync().Result)
            {
                if(c.UserID == userId)
                {
                    userComments.Add(c);
                }
            }
            return userComments;
        }
    }
}
