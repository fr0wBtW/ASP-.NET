namespace MoiteRecepti.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using MoiteRecepti.Data;
    using MoiteRecepti.Data.Common.Repositories;
    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Data;
    using MoiteRecepti.Web.ViewModels;
    using MoiteRecepti.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IGetCountsService countService;
        private readonly IRecipesService recipesService;

        //  private readonly ApplicationDbContext db;


        public HomeController(IGetCountsService countService, IRecipesService recipesService)
        {
            this.countService = countService;
            this.recipesService = recipesService;
            //   ApplicationDbContext db
            //   this.db = db;
        }

        //1.
        /* public IActionResult Index()
         {
             var viewModel = this.countService.GetCounts();
             return this.View(viewModel);
         } */

        //2.
        public IActionResult Index()
        {
            var countsDto = this.countService.GetCounts();

            //It is possible to wirte IndexViewModel like but we should inject it in the contructor:
            // var viewModel = this.mapper.Map<IndexViewModel>(countsDto);
            var viewModel = new IndexViewModel
            {
                CategoriesCount = countsDto.CategoriesCount,
                ImagesCount = countsDto.ImagesCount,
                IngredientsCount = countsDto.IngredientsCount,
                RecipesCount = countsDto.RecipesCount,
                RandomRecipes = this.recipesService.GetRandom<IndexPageRecipeViewModel>(4)
            };
            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
