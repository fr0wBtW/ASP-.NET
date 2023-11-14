using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MoiteRecepti.Services.Data
{
    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string,string>> GetAllAsKeyValuePairs();
    }
}
