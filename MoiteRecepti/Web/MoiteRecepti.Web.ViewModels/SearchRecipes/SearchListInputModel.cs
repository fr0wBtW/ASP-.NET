using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiteRecepti.Web.ViewModels.SearchRecipes
{
    public class SearchListInputModel
    {
        public IEnumerable<int> Ingredients { get; set; }
    }
}
