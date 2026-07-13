using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
