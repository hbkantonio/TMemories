using Core.Models.Dtos;
using Core.Models.Dtos.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Contracts.Service
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogDto>> GetCountries();
    }
}
