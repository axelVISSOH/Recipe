using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Admin : Person
    {
        [Required]
        public string FullName { get; set; }
    }
}
