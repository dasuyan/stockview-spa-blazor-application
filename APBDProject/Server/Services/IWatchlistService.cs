using APBDProject.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APBDProject.Server.Services
{
    public interface IWatchlistService
    {
        public Task<IEnumerable<Company>> GetSubscribed(string id);
        public Task AddToWatchlist(string id, Company company);
        public Task RemoveFromWatchlist(string id, Company company);
    }
}
