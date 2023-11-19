namespace MoiteRecepti.Services.Data

{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MoiteRecepti.Data.Common.Repositories;
    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Mapping;
    using MoiteRecepti.Web.ViewModels.Recipes;

    public class RecipesService : IRecipesService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;

        public RecipesService(
            IDeletableEntityRepository<Recipe> recipesRepository,
            IDeletableEntityRepository<Ingredient> ingredientsRepository)
        {
            this.recipesRepository = recipesRepository;
            this.ingredientsRepository = ingredientsRepository;
        }

        public async Task CreateAsync(CreateRecipeInputModel input, string userId, string imagePath)
        {
            var recipe = new Recipe
            {
                CategoryId = input.CategoryId,
                CookingTime = TimeSpan.FromMinutes(input.CookingTime),
                Instructions = input.Instructions,
                Name = input.Name,
                PortionsCount = input.PortionsCount,
                PreparationTime = TimeSpan.FromMinutes(input.PreparationTime),
                AddedByUserId = userId,
            };

            foreach (var inputIngredient in input.Ingredients)
            {
                var ingredient = this.ingredientsRepository.All().FirstOrDefault(x => x.Name == inputIngredient.IngredientName);
                if (ingredient == null)
                {
                    ingredient = new Ingredient { Name = inputIngredient.IngredientName };
                }

                recipe.Ingredients.Add(new RecipeIngredient
                {
                    Ingredient = ingredient,
                    Quantity = inputIngredient.Quantity,
                });
            }

            // /wwwroot/images/recipes/jhdsi-343g3h453-=g34g.jpg
            Directory.CreateDirectory($"{imagePath}/recipes/");
            foreach (var image in input.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new Image
                {
                    AddedByUserId = userId,
                    Extension = extension,
                };
                recipe.Images.Add(dbImage);

                var physicalPath = $"{imagePath}/recipes/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.recipesRepository.AddAsync(recipe);
            await this.recipesRepository.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
           var recipe = this.recipesRepository.All().FirstOrDefault(x => x.Id == id);
            this.recipesRepository.Delete(recipe);
            await this.recipesRepository.SaveChangesAsync();
        }

        /* Without AutoMapper
          public IEnumerable<RecipeInListViewModel> GetAll(int page, int itemsPerPage = 12)
           {
               var recipes = this.recipesRepository.AllAsNoTracking().OrderByDescending(x => x.Id).Skip(page - 1).Take(itemsPerPage).Select(x => new RecipeInListViewModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   CategoryName = x.Category.Name,
                   CategoryId = x.CategoryId,
                   ImageUrl = x.Images.FirstOrDefault().RemoteImageUrl != null ?
                   x.Images.FirstOrDefault().RemoteImageUrl : 
                   "/images/recipes/" + x.Images.FirstOrDefault().Id + ".0" + x.Images.FirstOrDefault().Extension
               }).ToList();

               return recipes;
           } */

        // With AutoMapper
        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12)
        {
            var recipes = this.recipesRepository.AllAsNoTracking().OrderByDescending(x => x.Id).Skip(page - 1).Take(itemsPerPage)
                .To<T>().ToList();

            return recipes;
        }
        public T GetById<T>(int id)
        {
            var recipe = this.recipesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();


            return recipe;
        }

        public IEnumerable<T> GetByIngredients<T>(IEnumerable<int> ingredientIds)
        {
            var query = this.recipesRepository.All().AsQueryable();
            foreach (var ingredientId in ingredientIds)
            {
                query = query.Where(x => x.Ingredients.Any(i => i.IngredientId == ingredientId));
            }

            return query.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.recipesRepository.All().Count();
        }

        public IEnumerable<T> GetRandom<T>(int count)
        {
            //Guid new Guid in EF = Random.Next();
            return this.recipesRepository.All().OrderBy(x => Guid.NewGuid()).Take(count).To<T>().ToList();
        }

        public async Task UpdateAsync(int id, EditRecipeInputModel input)
        {
            var recipes = this.recipesRepository.All().FirstOrDefault(x => x.Id == id);
            recipes.Name = input.Name;
            recipes.Instructions = input.Instructions;
            recipes.PreparationTime = TimeSpan.FromMinutes(input.PreparationTime);
            recipes.CookingTime = TimeSpan.FromMinutes(input.CookingTime);
            recipes.PortionsCount = input.PortionsCount;
            recipes.CategoryId = input.CategoryId;
            await this.recipesRepository.SaveChangesAsync();
        }
    }
}
