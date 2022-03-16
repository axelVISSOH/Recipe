using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace RecipeAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public bool IsReport { get; set; }
        public bool IsReportable { get; set; }
        [ForeignKey("FK_Recipe_Comment")]
        public int RecipeID { get; set; }
        [ForeignKey("FK_User_Comment")]
        public int UserID { get; set; }
    }
}