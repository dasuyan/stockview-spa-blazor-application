using System.Collections.Generic;
using System.Threading.Tasks;
using APBDProject.Shared.Models.DTOs;

namespace APBDProject.Server.Services
{
    public interface ICompanyService
    {
        public Task<IEnumerable<CompanyGet>> CompanySearchApi(string like);
        public Task<CompanyGet> GetCompany(string ticker);
        public Task<IEnumerable<OhlcForChart>> GetCompanyOhlc(string ticker);
        public Task AddCompany(CompanyWithOhlc ticker);
    }
}
