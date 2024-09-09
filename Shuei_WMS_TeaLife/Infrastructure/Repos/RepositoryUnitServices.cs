using Application.Extentions;
using Application.Services;
using Domain.Entity.Products;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class RepositoryUnitServices(ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor) : IUnit
    {
        public async Task<Result<List<Unit>>> GetAllAsync()
        {
            try
            {
                return await Result<List<Unit>>.SuccessAsync(await dbContext.Units.ToListAsync());
            }
            catch (Exception ex)
            {
                return await Result<List<Unit>>.FailAsync(ex.Message);
            }
        }

        public Task<Result<Unit>> GetByIdAsync([Path] Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Unit>> InsertAsync([Body] Unit model)
        {
            try
            {
                await dbContext.Units.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return await Result<Unit>.SuccessAsync(model);
            }
            catch (Exception ex)
            {
                return await Result<Unit>.FailAsync(ex.Message);
            }
        }

        public Task<Result<Unit>> UpdateAsync([Body] Unit model)
        {
            throw new NotImplementedException();
        }
    }
}
