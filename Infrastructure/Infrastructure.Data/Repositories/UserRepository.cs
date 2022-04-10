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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
