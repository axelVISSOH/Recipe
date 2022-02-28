using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;

namespace RecipeAPI.Data
{
    public class RecipeAPIContext : DbContext
    {
        public RecipeAPIContext (DbContextOptions<RecipeAPIContext> options)
            : base(options)
        {
        }

        public DbSet<RecipeAPI.Models.User> User { get; set; }

        public DbSet<RecipeAPI.Models.Recipe> Recipe { get; set; }

        public DbSet<RecipeAPI.Models.Step> Step { get; set; }

        public DbSet<RecipeAPI.Models.Instruction> Instruction { get; set; }

        public DbSet<RecipeAPI.Models.Ingredient> Ingredient { get; set; }
    }
}
