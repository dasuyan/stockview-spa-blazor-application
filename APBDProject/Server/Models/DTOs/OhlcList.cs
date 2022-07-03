using System.Collections.Generic;

namespace APBDProject.Server.Models.DTOs
{   //had to wrap JSON API response in this class instead of parametrizing it to List<SingleOhlc>.
    //Otherwise, it would not be possible to deserialize the JSON response into a List<SingleOhlc> object.
    public class OhlcList 
    {
        public List<OhlcGet> Results { get; set; }
    }
}
