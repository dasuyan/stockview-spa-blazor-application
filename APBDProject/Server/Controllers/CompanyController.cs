using System;
using APBDProject.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using APBDProject.Shared.Models.DTOs;

namespace APBDProject.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;
        public CompanyController(ICompanyService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IEnumerable<CompanyGet>> GetSearchTickers([FromQuery(Name = "$filter")] string filter, 
            [FromQuery(Name = "$skip")] int skip, [FromQuery(Name = "$top")] int top)
        {
            var likeSymbol = filter.Split("(")[1].Split("'")[1];
            var result = await _service.CompanySearchApi(likeSymbol);
            
            return result;
        }

        [HttpGet("companyInfo/{idCompany}")]
        public async Task<CompanyGet> GetCompanyInfo(string idCompany)
        {
            var result = await _service.GetCompany(idCompany);
            return result;
        }

        [HttpGet("ohlcInfo/{idOhlc}")]
        public async Task<IEnumerable<OhlcForChart>> GetOhlc(string idOhlc)
        {
            var result = await _service.GetCompanyOhlc(idOhlc);
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyAndOhlc(CompanyWithOhlc symbol)
        {
            try
            {
                await _service.AddCompany(symbol);
            }
            catch (Exception)
            {
                return Problem("Unknown server error");
            }
            return NoContent();
        }
    }
}
