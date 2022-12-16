using System.Collections.Generic;

namespace Recipes.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.Recipes = new HashSet<RecipeIngredient>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RecipeIngredient> Recipes{ get; set; }
    }
}
