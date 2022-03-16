using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Data;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeAPIContext _context;

        public RecipesController(RecipeAPIContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipe()
        {
            return await _context.Recipe.Include("Comments")
                                        .Include("Ingredients")
                                        .Include(x => x.Steps)
                                          .ThenInclude(steps => steps.Instructions)
                                        .ToListAsync();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id) ;

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // GET: api/GetRecipesByUserId/5
        [HttpGet("GetRecipeByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipeByUserId(int  id)
        {
            return await _context.Recipe
                                 .Where(r => r.UserID == id)                                        
                                        .Include("Steps")
                                        .Include("Comments")
                                        .Include("Ingredients")                                        
                                        .ToListAsync();
            
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            
            if (id != recipe.Id)
            {
                return BadRequest();
            }
            if (recipe != null && !recipe.Id.Equals(null))
            {
                updateIngredientList(recipe);
                updateStepsList(recipe);
            }           
            _context.Entry(recipe).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public void updateIngredientList(Recipe recipe)
        {
            foreach (var service in recipe.Ingredients)
            {
                if (!service.Id.Equals(0))
                    _context.Entry(service).State = EntityState.Modified;
            }

                ICollection<int> listIdRecipe = (recipe.Ingredients.Count() != 0)
                        ? recipe.Ingredients.Select(x => x.Id).ToList()
                        : new List<int>();

                ICollection<Ingredient> listIngredientToDelete = (listIdRecipe.Count() != 0)
                        ? _context.Ingredient.Where(r => (!listIdRecipe.Contains(r.Id) && r.RecipeID == recipe.Id)).ToList()
                        : _context.Ingredient.Where(r => r.RecipeID == recipe.Id).ToList();

                foreach (Ingredient i in listIngredientToDelete)
                {
                    _context.Entry(i).State = EntityState.Deleted;
                }

                ICollection<Ingredient> listIngredientToAdd = (recipe.Ingredients.Count() != 0)
                     ? recipe.Ingredients.Where(x => x.Id.Equals(0)).ToList()
                     : new List<Ingredient>();

                foreach (Ingredient i in listIngredientToAdd)
                {
                  _context.Entry(i).State = EntityState.Added;
                }

           

        }

        public void updateStepsList(Recipe recipe)
        {
            foreach (var s in recipe.Steps)
            {
                if (!s.Id.Equals(0))
                {
                    _context.Entry(s).State = EntityState.Modified;
                    updateInstructionList(s);
                }
                   
            }

           
                ICollection<int> listIdStep = (recipe.Steps.Count() != 0)
                        ? recipe.Steps.Select(x => x.Id).ToList()
                        : new List<int>();

                ICollection<Step> listStepToDelete = (listIdStep.Count() != 0)
                        ? _context.Step.Where(r => (!listIdStep.Contains(r.Id) && r.RecipeID == recipe.Id)).ToList()
                        : _context.Step.Where(r => r.RecipeID == recipe.Id).ToList();

                foreach (Step s in listStepToDelete)
                {
                    foreach (Instruction i in s.Instructions.ToList())
                    {
                        i.StepID = s.Id;
                        _context.Entry(i).State = EntityState.Deleted;
                    }
                _context.Entry(s).State = EntityState.Deleted;
                }

                ICollection<Step> listStepToAdd = (recipe.Steps.Count() != 0)
                     ? recipe.Steps.Where(x => x.Id.Equals(0)).ToList()
                     : new List<Step>();

                foreach (Step s in listStepToAdd)
                {
                    _context.Entry(s).State = EntityState.Added;
                    foreach (Instruction i in s.Instructions.ToList())
                    {
                        i.StepID = recipe.Id;
                        _context.Entry(i).State = EntityState.Added;
                    }
            }
        }

        public void updateInstructionList(Step step)
        {

            foreach (var i in step.Instructions)
            {
                if (!i.Id.Equals(0))
                    _context.Entry(i).State = EntityState.Modified;
            }

            ICollection<int> listIdInstruction = (step.Instructions.Count() != 0)
                    ? step.Instructions.Select(x => x.Id).ToList()
                    : new List<int>();

            ICollection<Instruction> listInstructionToDelete = (listIdInstruction.Count() != 0)
                    ? _context.Instruction.Where(r => (!listIdInstruction.Contains(r.Id) && r.StepID == step.Id)).ToList()
                    : _context.Instruction.Where(r => r.StepID == step.Id).ToList();

            foreach (Instruction i in listInstructionToDelete)
            {

                _context.Entry(i).State = EntityState.Deleted;
            }

            ICollection<Instruction> listInstructionToAdd = (step.Instructions.Count() != 0)
                 ? step.Instructions.Where(x => x.Id.Equals(0)).ToList()
                 : new List<Instruction>();

            foreach (Instruction i in listInstructionToAdd)
            {
                _context.Entry(i).State = EntityState.Added;
            }
        }

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipe.Add(recipe);
       
            foreach (Ingredient i in recipe.Ingredients)
            {
                i.RecipeID = recipe.Id;
                _context.Entry(i).State = EntityState.Added;
            }
            foreach (Step s in recipe.Steps)
            {
                s.RecipeID = recipe.Id;
                _context.Entry(s).State = EntityState.Added;
                foreach (Instruction i in s.Instructions.ToList())
                {
                    i.StepID = s.Id;
                    _context.Entry(i).State = EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            foreach (Ingredient i in recipe.Ingredients)
            {
                i.RecipeID = recipe.Id;
                _context.Entry(i).State = EntityState.Deleted;
            }
            foreach (Step s in recipe.Steps)
            {
                s.RecipeID = recipe.Id;
                _context.Entry(s).State = EntityState.Deleted;
                foreach (Instruction i in s.Instructions.ToList())
                {
                    i.StepID = s.Id;
                    _context.Entry(i).State = EntityState.Deleted;
                }
            }
            
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}
