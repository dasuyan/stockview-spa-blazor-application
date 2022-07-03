using APBDProject.Server.Data;
using APBDProject.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBDProject.Server.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext _context;
        public WatchlistService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Company>> GetSubscribed(string id)
        {
            var userInfo = await _context.Users.Include(e => e.Subscriptions).Where(e => e.UserName.Equals(id)).Select(e => new
            {
                watchedTickers = _context.Companies.Where(t => e.Subscriptions.Select(s => s.Company).Contains(t)).ToList()
            }).ToArrayAsync();

            var watchlist = userInfo.First().watchedTickers;
            return watchlist;
        }

        public async Task AddToWatchlist(string id, Company t)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var isTickerInDatabase = await _context.Companies.Where(e => e.Symbol.Equals(t.Symbol)).ToListAsync();
                if (isTickerInDatabase.Count == 0)
                {
                    await _context.Companies.AddAsync(t);
                    await _context.SaveChangesAsync();
                }

                var isSubscribed = await _context.Subscriptions.Where(e => e.CompanyName.Equals(t.Symbol) && e.IdUser.Equals(_context.Users.Where(u => u.UserName.Equals(id)).Select(u => u.Id).First())).ToListAsync();
                if (isSubscribed.Count != 0)
                    throw new Exception("It's already on your watchlist!");

                await _context.Subscriptions.AddAsync(new Subscription
                {
                    CompanyName = t.Symbol,
                    IdUser = _context.Users.Where(e => e.UserName.Equals(id)).Select(e => e.Id).First()
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
        
        public async Task RemoveFromWatchlist(string id, Company company)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var find = await _context.Subscriptions.FirstAsync(e => e.CompanyName.Equals(company.Symbol) && e.IdUser.Equals(_context.Users.First(u => u.UserName.Equals(id)).Id));
                find.Company = null;
                find.CompanyName = null;
                find.IdUser = null;
                find.User = null;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
