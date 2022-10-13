using CSharpFunctionalExtensions;
using System;

namespace Logic.Utils.Db
{
    public abstract class BaseEntity : Entity<int>
    {
        public DateTime CreateDate { get; protected set; }
        public DateTime? LastUpdateDate { get; protected set; }
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
