using Application.Extentions;
using Application.Models;
using Application.Services;
using Domain.Entity.Products;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace Infrastructure.Repos
{
    public class RepositoryProductServices(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IProduct
    {
        public async Task<Result<List<Product>>> GetAllAsync()
        {
            try
            {
                return await Result<List<Product>>.SuccessAsync(dbContext.Products.ToList());
            }
            catch (Exception ex)
            {
                return await Result<List<Product>>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<Product>> GetByIdAsync([Path] Guid id)
        {
            try
            {
                var result = await dbContext.Products.FindAsync(id);
                return await Result<Product>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                return await Result<Product>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<List<Product>>> GetFilterAsync([Body] Fillter model)
        {
            try
            {
                var d = await dbContext.Products
                    .Where<Product>(x => x.CategoryName == model.CategoryName).ToListAsync();
                return await Result<List<Product>>.SuccessAsync(d);
            }
            catch (Exception ex)
            {
                return await Result<List<Product>>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<Product>> InsertAsync([Body] Product model)
        {
            try
            {
                await dbContext.Products.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return await Result<Product>.SuccessAsync(model);
            }
            catch (Exception ex)
            {
                return await Result<Product>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<Product>> UpdateAsync([Body] Product model)
        {
            try
            {
                var dataUpdate= dbContext.Products.Update(model);
                await dbContext.SaveChangesAsync();
                return await Result<Product>.SuccessAsync(model);
            }
            catch (Exception ex)
            { 
                return await Result<Product>.FailAsync(ex.Message);
            }
        }
    }
}
