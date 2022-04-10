using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Contracts.Data
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }

        int Save();

        Task<int> SaveAsync();

        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
