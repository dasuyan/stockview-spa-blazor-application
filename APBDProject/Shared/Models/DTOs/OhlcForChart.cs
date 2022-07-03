using System;

namespace APBDProject.Shared.Models.DTOs
{   //had to keep it lowercase, otherwise the Syncfusion Stock Chart wouldn't load the data.
    public class OhlcForChart
    {
        public DateTime Date { get; set; }
        public double o { get; set; }
        public double h { get; set; }
        public double c { get; set; }
        public double l { get; set; }
        public double v { get; set; }
    }
}
