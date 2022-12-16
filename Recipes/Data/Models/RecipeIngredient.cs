namespace Recipes.Data.Models
{
    public class RecipeIngredient //междинна таблица за свързване на таблицата Recipe Ingredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public string Quantity { get; set; }
    }
}
