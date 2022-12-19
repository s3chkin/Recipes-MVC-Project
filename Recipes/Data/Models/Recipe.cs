using Microsoft.AspNetCore.Identity;
using Recipes.Models;
using System;
using System.Collections.Generic;

namespace Recipes.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Images = new HashSet<Image>();
            this.Ingredients = new HashSet<RecipeIngredient>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan PerparationTime { get; set; }
        public TimeSpan CookingTime { get; set; }
        public int PortionCount { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } //създаване на връзката
        public virtual ICollection<RecipeIngredient> Ingredients { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public ApplicationUser AddedByUserId { get; set; }
        public virtual ApplicationUser AddedByUser { get; set; }
    }
}
