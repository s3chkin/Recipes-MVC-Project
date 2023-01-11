using Recipes.Data;
using Recipes.Models;
using System;
using System.Linq;

namespace Recipes.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext db;
        public RecipeService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public InputRecipeModel GetRecipeById(int id)
        {
            var recipe = db.Recipes.Where(x => x.Id == id).Select(x => new InputRecipeModel
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.Category.Name,
                PreparationTime = TimeSpan.FromMinutes(x.PerparationTime.TotalMinutes).Minutes,
                CookingTime = TimeSpan.FromMinutes(x.CookingTime.TotalMinutes).Minutes,
                PortionCount = x.PortionCount,
                Description = x.Description,
                ImgURL = $"/img/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extention}"
            }).FirstOrDefault();

            return recipe;
        }
    }
}
