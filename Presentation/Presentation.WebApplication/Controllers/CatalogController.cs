using Core.Contracts.Service;
using Core.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            ResponseDto<IEnumerable<CatalogDto>> response = new ResponseDto<IEnumerable<CatalogDto>>();
            try
            {
                response.Data = await _catalogService.GetCountries();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }
    }
}
