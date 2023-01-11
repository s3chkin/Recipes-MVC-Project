using Recipes.Models;

namespace Recipes.Services
{
    public interface IRecipeService
    {
        public InputRecipeModel GetRecipeById(int id);
    }
}
