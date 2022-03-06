using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Step
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int Position { get; set; }
        public ICollection<Instruction> Instructions { get; set; }

        [ForeignKey("FK_Recipe_Step")]
        public int RecipeID { get; set; }
        public Step()
        {
            Instructions = new List<Instruction>();
        }
    }
}
