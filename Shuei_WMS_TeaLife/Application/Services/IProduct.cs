using Application.Extentions;
using Application.Models;
using Application.Services.Base;
using Domain.Entity.Products;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    [BasePath(ApiRoutes.Product.BasePath)]
    public interface IProduct : IRepository<Guid, Product>
    {
        [Get(ApiRoutes.Product.GetFillter)]
        Task<Result<List<Product>>> GetFilterAsync([Body] Fillter model);
    }
}
