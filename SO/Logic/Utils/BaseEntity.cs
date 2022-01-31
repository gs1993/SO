using CSharpFunctionalExtensions;
using System;

namespace Logic.Models
{
    public abstract class BaseEntity : Entity<int>
    {
        public DateTime CreateDate { get; private set; }
        public DateTime? LastUpdateDate { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeleteDate { get; private set; }

        public void SetCreateDate(DateTime createDate)
        {
            CreateDate = createDate;
        }

        public void SetUpdateDate(DateTime updateDate)
        {
            LastUpdateDate = updateDate;
        }

        public void Delete(DateTime deleteDate)
        {
            if (IsDeleted)
                return;
            IsDeleted = true;
            DeleteDate = deleteDate;
        }
    }
}
