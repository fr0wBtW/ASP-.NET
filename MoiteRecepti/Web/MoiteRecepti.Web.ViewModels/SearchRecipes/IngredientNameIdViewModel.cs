namespace MoiteRecepti.Web.ViewModels.SearchRecipes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Mapping;

    public class IngredientNameIdViewModel : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
