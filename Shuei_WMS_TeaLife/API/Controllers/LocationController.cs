using API.Controllers.Base;
using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.Extentions;
using Application.Services;
using Domain.Entity;
using Infrastructure.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController<Guid,Location>,ILocation
    {
        readonly Repository _repository;

        public LocationController(Repository repository = null) : base(repository.SLocation)
        {
            _repository = repository;
        }

        [HttpPost(ApiRoutes.Locations.DeleteLocation)]
        public async Task<Result<GeneralResponse>> DeleteLocationAsync(UpdateDeleteRequestDTO model)
        {
            return await _repository.SLocation.DeleteLocationAsync(model);
        }
    }
}
