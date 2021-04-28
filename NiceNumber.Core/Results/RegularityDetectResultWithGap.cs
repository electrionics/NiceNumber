using System.Collections.Generic;

namespace NiceNumber.Results
{
    public class RegularityDetectResultWithGap:RegularityDetectResult
    {
        #region Equals
        
        protected bool Equals(RegularityDetectResultWithGap other)
        {
            return base.Equals(other) && Gap == other.Gap;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RegularityDetectResultWithGap) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Gap.GetHashCode();
            }
        }
        
        #endregion

        #region Comparers
        
        private sealed class EqualityComparer : IEqualityComparer<RegularityDetectResultWithGap>
        {
            public bool Equals(RegularityDetectResultWithGap x, RegularityDetectResultWithGap y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Equals(y);
            }

            public int GetHashCode(RegularityDetectResultWithGap obj)
            {
                return obj.GetHashCode();
            }
        }

        public static IEqualityComparer<RegularityDetectResultWithGap> Comparer { get; } = new EqualityComparer();
        
        #endregion

        public byte Gap { get; set; }
    }
}