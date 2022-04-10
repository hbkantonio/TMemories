using Core.Contracts.Data;
using Core.Contracts.Repositories;
using Core.Models.TMemoriesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
