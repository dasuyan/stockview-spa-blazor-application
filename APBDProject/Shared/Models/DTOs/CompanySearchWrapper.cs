using System.Collections.Generic;

namespace APBDProject.Shared.Models.DTOs
{
    public class CompanySearchWrapper
    {
        public IEnumerable<CompanyGet> Results  { get; set; }
        public string Status { get; set; }
    }
}
