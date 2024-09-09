using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class Repository
    {
        public IProduct SProduct { get; set; }
        public IUnit SUnit { get; set; }

        public Repository(IProduct sProduct = null, IUnit sUnit = null)
        {
            SProduct = sProduct;
            SUnit = sUnit;
        }
    }
}
