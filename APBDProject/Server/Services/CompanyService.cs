using APBDProject.Server.Data;
using APBDProject.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using APBDProject.Server.Models.DTOs;
using APBDProject.Shared.Models.DTOs;

namespace APBDProject.Server.Services
{
    public class CompanyService : ICompanyService
    {
        private static readonly HttpClient Http = new();
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCompany(CompanyWithOhlc ticker)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var isTickerInDatabase = await _context.Companies.Where(e => e.Symbol.Equals(ticker.ticker.Symbol)).ToListAsync();
                if (isTickerInDatabase.Count == 0)
                {
                    await _context.Companies.AddAsync(new Company
                    {
                        Symbol = ticker.ticker.Symbol,
                        Name = ticker.ticker.Name,
                        Country = new RegionInfo(ticker.ticker.Country).DisplayName,
                        Exchange = ticker.ticker.Exchange,
                        Market = ticker.ticker.Market,
                        Description = ticker.ticker.Description,
                        HomepageUrl = ticker.ticker.HomepageUrl
                    });
                    await _context.SaveChangesAsync();
                }
                
                var oldOhlcs = await _context.Ohlcs.Where(e => e.Symbol.Equals(ticker.ticker.Symbol)).ToListAsync();
                if(oldOhlcs.Count != 0)
                {
                    oldOhlcs.ForEach(e =>
                    {
                        e.Company = null;
                        e.Symbol = null;
                    });
                    await _context.SaveChangesAsync();
                    _context.RemoveRange(oldOhlcs);
                    await _context.SaveChangesAsync();
                }

                await _context.Ohlcs.AddRangeAsync(ticker.ohlcs.Select(e => new Ohlc
                {
                    Symbol = ticker.ticker.Symbol,
                    Date = e.Date,
                    O = e.o,
                    H = e.h,
                    L = e.l,
                    C = e.c,
                    V = e.v
                }));
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        public async Task<IEnumerable<CompanyGet>> CompanySearchApi(string like)
        {
            var response = await Http.GetFromJsonAsync<CompanySearchWrapper>($"https://api.polygon.io/v3/reference/tickers?active=true&sort=ticker&order=asc&limit=0&search={like}&apiKey=Nt65fJsU3DPbDKDtvbNKhVhgl2GEv0rg");
            return response.Results;
        }

        public async Task<CompanyGet> GetCompany(string symbol)
        {
            try
            {
                var json = await Http.GetFromJsonAsync<CompanyInfoResult>($"https://api.polygon.io/v3/reference/tickers/{symbol}?apiKey=Nt65fJsU3DPbDKDtvbNKhVhgl2GEv0rg");
                var tickerInfo = json.results;
                return tickerInfo;
            }
            catch(Exception)
            {
                return await _context.Companies.Where(e => e.Symbol.Equals(symbol)).Select(e => new CompanyGet
                {
                    Symbol = e.Symbol,
                    Name = e.Name,
                    Country = e.Country,
                    Exchange = e.Exchange,
                    Market = e.Market,
                    Description = e.Description,
                    HomepageUrl = e.HomepageUrl
                }).FirstAsync();
            }
        }

        public async Task<IEnumerable<OhlcForChart>> GetCompanyOhlc(string ticker)
        {
            try
            {
                var currentDate = DateTime.Now;
                var startingDate = DateTime.Today.AddMonths(-3);
                
                var apiResponse = await Http.GetFromJsonAsync<OhlcList>($"https://api.polygon.io/v2/aggs/ticker/{ticker}/range/1/day/{startingDate:yyyy-MM-dd}/{currentDate:yyyy-MM-dd}?adjusted=true&sort=asc&limit=120&apiKey=Nt65fJsU3DPbDKDtvbNKhVhgl2GEv0rg");
               
                var ohlcList = apiResponse.Results;
                if (ohlcList == null)
                    throw new Exception("No ohlc");
            
                var numberOfDays = (currentDate - startingDate).TotalDays;
                
                var record = ohlcList.Count;
                var Data = new List<OhlcForChart>();
                foreach (var o in ohlcList)
                {
                    Data.Add(new OhlcForChart
                    {
                        Date = startingDate.AddDays(numberOfDays / record),
                        o = o.O,
                        h = o.H,
                        c = o.C,
                        l = o.L,
                        v = o.V
                    });

                    startingDate = startingDate.AddDays(numberOfDays / record);
                }
                return Data;
            }
            catch
            {
                return await _context.Ohlcs.Where(e => e.Symbol.Equals(ticker)).Select(e => new OhlcForChart
                {
                    Date = e.Date,
                    o = e.O,
                    h = e.H,
                    l = e.L,
                    c = e.C,
                    v = e.V
                }).ToListAsync();
            }
        }
    }
}
