using Application.Extentions;
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
    [BasePath(ApiRoutes.Unit.BasePath)]
    public interface IUnit : IRepository<Guid,Unit>
    {
    }
}
