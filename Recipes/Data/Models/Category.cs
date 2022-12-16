using System.Collections.Generic;

namespace Recipes.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Recipe> Recipes{ get; set; } //колекция(списък) от рецепти, осъществяване на връзка между таблиците 

    }
}
