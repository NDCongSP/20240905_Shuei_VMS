using Application.Extentions;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public interface IRepository<TId, T> where T : class
    {
        [Get(ApiRoutes.GetAll)]
        Task<Result<List<T>>> GetAllAsync();

        [Get(ApiRoutes.GetById)]
        Task<Result<T>> GetByIdAsync([Path] TId id);

        [Post(ApiRoutes.Insert)]
        Task<Result<T>> InsertAsync([Body] T model);

        [Post(ApiRoutes.Update)]
        Task<Result<T>> UpdateAsync([Body] T model);
    }
}
