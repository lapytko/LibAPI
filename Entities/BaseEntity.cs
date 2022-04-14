using System;
using LibAPI.Attributes;

namespace LibAPI.Entities
{
    public class BaseEntity : IBaseEntity<string>
    {
        public TSelf InitId<TSelf>() where TSelf:BaseEntity
        {
            Id = Guid.NewGuid().ToString();
            return (TSelf) this;
        }
        public string Id { get; set; }
        public bool Visible { get; set; } = true;
        public bool IsDeleted { get; set; }
        public bool IsNew => Id == default;
    }
        
    public interface IBaseEntity<TKey> where TKey: IEquatable<TKey>
    {
        public TKey Id { get; set; }
        [IgnoreProperty]
        public bool Visible { get; set; }
        [IgnoreProperty]
        public bool IsDeleted { get; set; }
    }
}
