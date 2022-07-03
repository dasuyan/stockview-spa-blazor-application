using APBDProject.Server.Models;
using APBDProject.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APBDProject.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _service;
        
        public WatchlistController(IWatchlistService service)
        {
            _service = service;
        }
        
        [HttpGet("{idUser}")]
        public async Task<IEnumerable<Company>> GetSubscribedAsync(string idUser)
        {
            return await _service.GetSubscribed(idUser);
        }

        [HttpPost("{idUser}")]
        public async Task<IActionResult> AddToWatchlistAsync(string idUser, Company company)
        {
            try
            {
                await _service.AddToWatchlist(idUser, company);
            }
            catch (Exception)
            {
                return Problem("Unknown server error");
            }
            return NoContent();
        }

        [HttpPost("delete/{idUser}")]
        public async Task<IActionResult> RemoveFromWatchlistAsync(string idUser, Company company)
        {
            try
            {
                await _service.RemoveFromWatchlist(idUser, company);
            }
            catch (Exception)
            {
                return Problem("Unknown server error");
            }
            return NoContent();
        }
    }
}
