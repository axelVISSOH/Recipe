using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeWebClient.Models
{
    public class User : Person
    {
        public string Pseudo { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public User()
        {
            Recipes = new List<Recipe>();
            Comments = new List<Comment>();
        }
    }
}
