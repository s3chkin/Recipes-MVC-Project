﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Recipes.Models
{
    public class InputRecipeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public IFormFile Image { get; set; }
        public int CookingTime { get; set; }
        public int PortionCount { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<InputRecipeIngridientModel> Ingridients { get; set; }
        public string ImgURL{ get; set; }
    }
}
