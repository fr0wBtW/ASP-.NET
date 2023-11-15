using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiteRecepti.Services.Data
{
    public interface IIngredientsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
