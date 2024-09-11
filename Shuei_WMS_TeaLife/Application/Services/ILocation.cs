using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.Extentions;
using Application.Services.Base;
using Domain.Entity;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    [BasePath(ApiRoutes.Locations.BasePath)]
    public interface ILocation:IRepository<Guid,Location>
    {
        [Post(ApiRoutes.Locations.DeleteLocation)]
        Task<Result<GeneralResponse>> DeleteLocationAsync(UpdateDeleteRequestDTO model); 
    }
}
