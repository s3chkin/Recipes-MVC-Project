using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Models
{
    public class InputRecipeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(0,300)]
        public int PreparationTime { get; set; }
        public IFormFile Image { get; set; }
        [Range(0, 300)]
        public int CookingTime { get; set; }
        [Range(0, 100)]
        public int PortionCount { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<InputRecipeIngridientModel> Ingridients { get; set; }
        public string ImgURL{ get; set; }
    }
}
