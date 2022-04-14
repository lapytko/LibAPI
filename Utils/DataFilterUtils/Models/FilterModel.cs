using System.Collections.Generic;

namespace LibAPI.Utils.DataFilterUtils.Models
{
    public class FilterModel
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public IEnumerable<Sort> Sort { get; set; }
        public Filter Filter { get; set; }
    }

    
    public class FilterSyncModel : FilterModel
    {
        public bool IsSync { get; set; } = false;
    }

}