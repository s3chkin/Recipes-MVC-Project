using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recipes.Data;
using Recipes.Data.Models;
using Recipes.Models;
using System;
using System.IO;
using System.Linq;

namespace Recipes.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private string[] allowedExtention = new[] { "png", "jpg", "jpeg" };

        public RecipeController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var model = db.Recipes.Select(x => new InputRecipeModel
            {
                Name = x.Name,
                Id = x.Id,
                ImgURL = $"/img/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extention}", //четене на снимката от базата данни
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            //
            var categories = db.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            InputRecipeModel model = new InputRecipeModel
            {
                Categories = categories
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(InputRecipeModel model)
        {
            //Добавяне на 1 рецепта в базата данни
            var recipe = new Recipe
            {
                Name = model.Name,
                PerparationTime = TimeSpan.FromMinutes(model.PreparationTime),
                CookingTime = TimeSpan.FromMinutes(model.CookingTime),
                PortionCount = model.PortionCount,
                CategoryId = model.CategoryId,
                Description = model.Description
            };
            // от името на прикачения файл получаваме неговото разширение   .png
            var extention = Path.GetExtension(model.Image.FileName).TrimStart('.');
            //създаваме обект, който ще се запише в БД
            var image = new Image
            {
                Extention = extention
            };
            string path = $"{webHostEnvironment.WebRootPath}/img/{image.Id}.{extention}";

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                model.Image.CopyTo(fs);
            }
            recipe.Images.Add(image);
            foreach (var item in model.Ingridients)
            {
                var ingridient = db.Ingredients.FirstOrDefault(x => item.Name == x.Name);
                if (ingridient == null)
                {
                    ingridient = new Ingredient
                    {
                        Name = item.Name,
                    };
                }

                recipe.Ingredients.Add(new RecipeIngredient
                {
                    Ingredient = ingridient,
                    Quantity = item.Quantity
                });
            }
            db.Recipes.Add(recipe);
            db.SaveChanges();

            return this.Redirect("/");
        }
        public IActionResult ById(int id)
        {
            var model = db.Recipes.Where(x => id == x.Id).Select(x => new InputRecipeModel
            {
                //otlyavo modeli otdyasno bazatadanni
                Name = x.Name,
                CategoryName = x.Category.Name,
                PreparationTime = TimeSpan.FromMinutes(x.PerparationTime.TotalMinutes).Minutes,
                CookingTime = TimeSpan.FromMinutes(x.CookingTime.TotalMinutes).Minutes,
                PortionCount = x.PortionCount,
                Description = x.Description,
                ImgURL = $"/img/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extention}", //четене на снимката от базата данни
                Ingridients = x.Ingredients.Select(x => new InputRecipeIngridientModel
                {
                    Name = x.Ingredient.Name,
                    Quantity = x.Quantity
                }).ToList()
            }).FirstOrDefault();
            //var category = db.Categories.Where(x => model.CategoryId == x.Id).FirstOrDefault();
            return this.View(model);
        }

        //public IActionResult Edit(int id)
        //{

        //    var recipe = db.Recipes.Where(s => s.Id == id).FirstOrDefault();
        //    var model = new InputRecipeModel
        //    {
        //        Id = recipe.Id,
        //        Name = recipe.Name,
               
        //    };
        //    return this.View(model);
        //}
        //[HttpPost]
        //public IActionResult Edit(InputRecipeModel model) //update
        //{
        //    var recipe = db.Recipes.Where(s => s.Id == model.Id).FirstOrDefault(); //търсене
        //    recipe.Name = model.Name;
        //    db.SaveChanges();
        //    return this.RedirectToActionPermanent("Index");
        //}

        public IActionResult Delete(int id)
        {
            var task = db.Recipes.Where(s => s.Id == id).FirstOrDefault(); //търсене
            db.Recipes.Remove(task);
            db.SaveChanges();
            return this.RedirectToAction("Index");
        }
    }
}
