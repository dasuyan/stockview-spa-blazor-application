using System.Collections.Generic;

namespace APBDProject.Shared.Models.DTOs
{
    public class CompanyWithOhlc
    {
        public CompanyGet ticker { get; set; }
        public ICollection<OhlcForChart> ohlcs { get; set; }
    }
}
