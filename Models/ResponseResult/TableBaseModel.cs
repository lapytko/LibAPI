using System.Collections.Generic;

namespace LibAPI.Models.ResponseResult
{
    public class TableBaseModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Count { get; set; }
    }
}