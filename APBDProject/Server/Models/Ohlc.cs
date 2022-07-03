using System;

namespace APBDProject.Server.Models
{
    public class Ohlc
    {
        public int IdOhlc { get; set; }
        public string Symbol { get; set; }

        public DateTime Date { get; set; }
        public double O { get; set; }
        public double C { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double V { get; set; }

        public virtual Company Company { get; set; }
    }
}
