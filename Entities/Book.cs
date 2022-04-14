using System;
using System.Collections.Generic;
using LibAPI.Entities.Enums;

namespace LibAPI.Entities
{
    public class Book:BaseEntity
    {
        public string? ISBN { get; set; }
        public string? IdentNumber { get; set; }
        public string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public short? PublishYear { get; set; }
        
        public List<Author> Author { get; set; }
        public Publisher Publisher { get; set; }
        public List<Style>  Style { get; set; }
        
        public CoverType CoverType { get; set; }
        public BookType Type { get; set; }
        public bool  PayType { get; set; }
        public BookStatus Status { get; set; }
        
        public double? CostPerDay { get; set; }
        
        public List<LendingItem> LendingItem { get; set; }
        
        
    }
}
