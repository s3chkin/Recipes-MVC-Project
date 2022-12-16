using System;

namespace Recipes.Data.Models
{
    public class Image
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; } //imeto na snimkata
        public string Extention { get; set; }
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
