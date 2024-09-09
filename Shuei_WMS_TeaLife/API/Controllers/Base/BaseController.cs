using Application.Extentions;
using Application.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestEase;

namespace API.Controllers.Base
{
    //[Authorize]//không cần xét role, login vào là gọi đc API    
    [Route("api/[controller]")]
    [ApiController]
    //[ApiController,JsonifyErrors]
    public class BaseController<TId, T> : ControllerBase, IRepository<TId, T> where T : class
    {
        readonly IRepository<TId, T> _repository;

        public BaseController(IRepository<TId, T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<Result<List<T>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet(ApiRoutes.GetById)]
        public async Task<Result<T>> GetByIdAsync([Path] TId id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpPost(ApiRoutes.Insert)]
        public async Task<Result<T>> InsertAsync([Body] T model)
        {
            return await _repository.InsertAsync(model);
        }

        [HttpPost(ApiRoutes.Update)]
        public async Task<Result<T>> UpdateAsync([Body] T model)
        {
            return await _repository.UpdateAsync(model);
        }
    }
}