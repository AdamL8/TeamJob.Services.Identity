using System;
using System.Collections.Generic;
using System.Text;
using TeamJob.Services.Identity.Core.Exceptions;

namespace TeamJob.Services.Identity.Core.Entities
{
    public class AggregateId : IEquatable<AggregateId>
    {
        public string Value { get; }

        public AggregateId()
        {
            Value = Guid.NewGuid().ToString();
        }

        public AggregateId(string value)
        {
            if (value == string.Empty)
            {
                throw new InvalidAggregateIdException();
            }

            Value = value;
        }

        public bool Equals(AggregateId other)
        {
            if (ReferenceEquals(null, other))
            { return false; }

            return ReferenceEquals(this, other) || Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            { return false; }

            if (ReferenceEquals(this, obj))
            { return true; }

            return obj.GetType() == GetType() && Equals((AggregateId)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(AggregateId id)
            => id.Value;

        public static implicit operator AggregateId(string id)
            => new AggregateId(id);
    }
}
