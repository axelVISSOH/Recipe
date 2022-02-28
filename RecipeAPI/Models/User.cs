using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Pseudo { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        public User()
        {
            Recipes = new List<Recipe>();
        }

    }
}
