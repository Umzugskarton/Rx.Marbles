using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Model
{
    public abstract class BaseId
    {
        public string Id { get; }

        public BaseId(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
        public BaseId() 
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public bool Equals(BaseId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseId)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
