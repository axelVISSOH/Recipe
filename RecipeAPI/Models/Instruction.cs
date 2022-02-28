using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Instruction
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Position { get; set; }

        [ForeignKey("FK_Step_Instruction")]
        public int StepID { get; set; }

    }
}
