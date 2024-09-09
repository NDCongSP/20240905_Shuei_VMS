using Application.Extentions;
using Domain.Entity.Authentication;
using Domain.Entity.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace Infrastructure.Data
{
    /// <summary>
    /// Seeding data ban đầu.
    /// </summary>
    public class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            // Ensure the database is created (or already exists), chỉ chạy 1 lần, sau đó nếu có sự thay đổi Entity thì nó ko cập nhật DB, nên nếu bật thì chỉ bật khi chạy runTime
            //context.Database.EnsureCreated();

            //chạy migration theo các migration đc tạo trong lúc build code.
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
            context.SaveChanges();

            string[] categoryName = { "Categorys 1", "Categorys 2", "Categorys 3", "Categorys 4", "Categorys 5", "Categorys 6" };
            string[] productName = { "Product 1", "Product 2", "Product 3", "Product 4", "Product 5", "Product 6" };

            var units = new List<Unit>();
            units.Add(new Unit()
            {
                Id = Guid.NewGuid(),
                Name = "Box",
                CreatedDate = DateTime.Now,
                CreatedBy = "SeedingData",
                IsActived = true,
            });
            units.Add(new Unit()
            {
                Id = Guid.NewGuid(),
                Name = "Pcs",
                CreatedDate = DateTime.Now,
                CreatedBy = "SeedingData",
                IsActived = true,
            });

            var categorys = new List<Category>();
            for (int i = 0; i < 6; i++)
            {
                categorys.Add(new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = categoryName[i],
                    CreatedDate = DateTime.Now,
                    CreatedBy = "SeedingData",
                    IsActived = true,
                });
            }

            var products = new List<Product>();
            for (int i = 0; i < 6; i++)
            {
                products.Add(new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = productName[i],
                    IdUnit = units[0].Id,
                    UnitName = units[0].Name,
                    IdCategory = categorys[i].Id,
                    CategoryName = categorys[i].Name,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "SeedingData",
                    IsActived = true,
                });
            }

            // Check if there are any products already present
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new Microsoft.AspNetCore.Identity.IdentityRole()
                {
                    Name = ConstantExtention.Roles.SupperAdmin,
                    NormalizedName=ConstantExtention.Roles.SupperAdmin.ToUpper()
                });
                await context.Roles.AddAsync(new Microsoft.AspNetCore.Identity.IdentityRole()
                {
                    Name = ConstantExtention.Roles.Admin,
                    NormalizedName = ConstantExtention.Roles.Admin.ToUpper()
                });
                await context.Roles.AddAsync(new Microsoft.AspNetCore.Identity.IdentityRole()
                {
                    Name = ConstantExtention.Roles.Operator,
                    NormalizedName = ConstantExtention.Roles.Operator.ToUpper()
                });
            }

            if (!context.Permissions.Any())
            {
                await context.AddAsync(new Permissions()
                {
                    Id = Guid.NewGuid(),
                    Name = "Add",
                    Description = "Can add data"
                });
                await context.AddAsync(new Permissions()
                {
                    Id = Guid.NewGuid(),
                    Name = "Update",
                    Description = "Can update data"
                });
                await context.AddAsync(new Permissions()
                {
                    Id = Guid.NewGuid(),
                    Name = "Delete",
                    Description = "Can delete data"
                });
            }

            if (!context.Units.Any())
            {
                await context.Units.AddRangeAsync(units);
            }

            if (!context.Categorys.Any())
            {
                await context.Categorys.AddRangeAsync(categorys);
            }

            if (!context.Products.Any())
            {
                await context.Products.AddRangeAsync(products);
            }

            // Save the changes to the database
            await context.SaveChangesAsync();
        }
    }
}
