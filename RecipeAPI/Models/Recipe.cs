using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public ICollection<Step> Steps { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }

        [ForeignKey("FK_User_Recipe")]
        public int UserID { get; set; }
        public Recipe()
        {
            Steps = new List<Step>();
            Ingredients = new List<Ingredient>();
        }
    }
}
