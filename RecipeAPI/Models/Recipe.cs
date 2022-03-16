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
        public string ImagePath { get; set; }
        public bool IsPublic { get; set; }
       
        public  ICollection<Comment> Comments { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Step> Steps { get; set; }
      
        [ForeignKey("FK_User_Recipe")]
        public int UserID { get; set; }
        public Recipe()
        {
            Comments = new List<Comment>();
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }
    }
}
