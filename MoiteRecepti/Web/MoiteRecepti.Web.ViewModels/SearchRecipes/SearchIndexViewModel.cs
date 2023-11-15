namespace MoiteRecepti.Web.ViewModels.SearchRecipes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchIndexViewModel
    {
        public IEnumerable<IngredientNameIdViewModel> Ingredients { get; set; }
    }
}
