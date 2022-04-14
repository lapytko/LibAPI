using System.Collections.Generic;
using LibAPI.Utils.DataFilterUtils.Enums;

namespace LibAPI.Utils.DataFilterUtils.Models
{
    public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; }
        public MatchCaseEnum Case { get; set; }
        public IEnumerable<Filter> Filters { get; set; }
    }
}