using Core.Business.Auth;
using Core.Contracts.Data;
using Core.Contracts.Repositories;
using Core.Contracts.Service;
using Core.Models.Dtos;
using Core.Models.Dtos.Accounts;
using Core.Models.TMemoriesModels;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Transversal.Helpers;

namespace Core.Business.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICountryRepository _countryRepository;

        public CatalogService(
            ICountryRepository countryRepository
            )
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<CatalogDto>> GetCountries()
        {
            return (await _countryRepository.Get()).Select(c => new CatalogDto { Text = c.Name, Value = c.Id });
        }
    }
}
