﻿using MoiteRecepti.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoiteRecepti.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "Тарт" });
            await dbContext.Categories.AddAsync(new Category { Name = "Кекс" });
            await dbContext.Categories.AddAsync(new Category { Name = "Печено Свинско" });

            await dbContext.SaveChangesAsync();
        }
    }
}
