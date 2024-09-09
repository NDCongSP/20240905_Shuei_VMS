using API.Controllers.Base;
using Application.Services;
using Application.Services.Base;
using Domain.Entity.Products;
using Infrastructure.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : BaseController<Guid, Unit>, IUnit
    {
        readonly Repository _repository;
        public UnitController(Repository repository) : base(repository.SUnit)
        {
            _repository = repository;
        }
    }
}
