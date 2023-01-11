using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recipes.Data;
using Recipes.Data.Models;
using Recipes.Models;
using Recipes.Services;
using System;
using System.IO;
using System.Linq;

namespace Recipes.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IShortStringService shortStringService;
        private readonly IRecipeService recipeService;
        private string[] allowedExtention = new[] { "png", "jpg", "jpeg" };

        public RecipeController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager, IShortStringService shortStringService,
            IRecipeService recipeService)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.shortStringService = shortStringService; //injektirane
            this.recipeService = recipeService; //injektirane
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
                Description = shortStringService.GetShort(x.Description, 4),
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = recipeService.GetRecipeById(id);
            var categories = db.Categories.Select(x =>
            new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.Categories = categories;
            return this.View(model);
        }
        [HttpPost]
        public IActionResult Edit(int id, InputRecipeModel model) //update
        {
            var recipe = db.Recipes.FirstOrDefault(x=>x.Id == ); //търсене
            recipe.Name = model.Name;
            recipe.PerparationTime =TimeSpan.FromMinutes(model.PreparationTime) ;
            recipe.CookingTime =TimeSpan.FromMinutes(model.CookingTime);
            recipe.Description = model.Description;
            recipe.CategoryId = model.CategoryId;
            recipe.PortionCount = model.PortionCount;
            db.SaveChanges();
            //return this.RedirectToActionPermanent("Index");
            return this.Redirect("Index");
        }

        public IActionResult Delete(int id)
        {
            var task = db.Recipes.Where(s => s.Id == id).FirstOrDefault(); //търсене
            db.Recipes.Remove(task);
            db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        //public JsonResult User()
        //{
        //    var user = userManager.GetUserAsync(User);
        //    return this.Json();
        //}
    }
}


















