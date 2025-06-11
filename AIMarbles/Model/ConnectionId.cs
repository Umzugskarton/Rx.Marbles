namespace AIMarbles.Model
{
    public class ConnectionId
    {
        public string Id { get; }

        public ConnectionId(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public bool Equals(ConnectionId other)
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
            return Equals((ConnectionId)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString() => $"ConnectionId: {Id}";
    }
}
