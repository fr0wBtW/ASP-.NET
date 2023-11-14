using MoiteRecepti.Services.Data.Dtos;
using MoiteRecepti.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiteRecepti.Services.Data
{
    public interface IGetCountsService
    {
        // 1. Use view model
        // IndexViewModel GetCounts();

        // 2. Create DTO -> view model
          CountsDto GetCounts();
    }
}
