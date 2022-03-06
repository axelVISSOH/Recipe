using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class User : Person
    {
        [Required]
        public string Pseudo { get; set; }        
        public ICollection<Recipe> Recipes { get; set; }
        public User()
        {
            Recipes = new List<Recipe>();
        }
    }
}