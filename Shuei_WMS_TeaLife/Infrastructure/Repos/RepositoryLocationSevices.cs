using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.Extentions;
using Application.Services;
using Domain.Entity;
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
    public class RepositoryLocationSevices(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : ILocation
    {
        public async Task<Result<GeneralResponse>> DeleteLocationAsync(UpdateDeleteRequestDTO model)
        {
            try
            {
                return await Result<GeneralResponse>.SuccessAsync(new GeneralResponse());
            }
            catch (Exception ex)
            {
                return await Result<GeneralResponse>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<List<Location>>> GetAllAsync()
        {
            try
            {
                return await Result<List<Location>>.SuccessAsync(await dbContext.Locations.ToListAsync());
            }
            catch (Exception ex)
            {
                return await Result<List<Location>>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<Location>> GetByIdAsync([Path] Guid id)
        {
            try
            {
                var result = await dbContext.Locations.FindAsync(id);
                return await Result<Location>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                return await Result<Location>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<Location>> InsertAsync([Body] Location model)
        {
            try
            {
                await dbContext.Locations.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return await Result<Location>.SuccessAsync(model);
            }
            catch (Exception ex)
            {
                return await Result<Location>.FailAsync(ex.Message);
            }
        }

        public async Task<Result<Location>> UpdateAsync([Body] Location model)
        {
            try
            {
                var dataUpdate = dbContext.Locations.Update(model);
                await dbContext.SaveChangesAsync();
                return await Result<Location>.SuccessAsync(model);
            }
            catch (Exception ex)
            {
                return await Result<Location>.FailAsync(ex.Message);
            }
        }
    }
}
