using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        public string Status { get; set; }
    }
}