using System.Collections.Generic;

namespace NiceNumber.Results
{
    public class RegularityDetectResult
    {
        #region Equals
        
        protected bool Equals(RegularityDetectResult other)
        {
            return Type == other.Type && FirstNumber == other.FirstNumber && FirstPosition == other.FirstPosition && Length == other.Length && RegularityNumber == other.RegularityNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RegularityDetectResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode * 397) ^ FirstNumber;
                hashCode = (hashCode * 397) ^ FirstPosition;
                hashCode = (hashCode * 397) ^ Length;
                hashCode = (hashCode * 397) ^ RegularityNumber;
                return hashCode;
            }
        }

        #endregion

        #region Comparers
        
        private sealed class RegularityDetectResultEqualityComparer : IEqualityComparer<RegularityDetectResult>
        {
            public bool Equals(RegularityDetectResult x, RegularityDetectResult y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Equals(y);
            }

            public int GetHashCode(RegularityDetectResult obj)
            {
                return obj.GetHashCode();
            }
        }

        public static IEqualityComparer<RegularityDetectResult> Comparer { get; } = new RegularityDetectResultEqualityComparer();
        
        #endregion
        
        public RegularityType Type { get; set; }
        
        public int FirstNumber { get; set; } // not required
        
        public int FirstPosition { get; set; } // required
        
        public int Length { get; set; } // required, min = 3
        
        public int RegularityNumber { get; set; } // number of regularity TODO: remove here and add in other special class
    }
}