using System.Collections.Generic;
using System.Linq;

namespace NiceNumber.Results
{
    public class RegularityDetectResultWithPositions:RegularityDetectResult
    {
        #region Equals

        protected bool Equals(RegularityDetectResultWithPositions other)
        {
            return base.Equals(other) && Positions.SequenceEqual(other.Positions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RegularityDetectResultWithPositions) obj);
        }

        #endregion
        
        #region Comparers
        
        private sealed class RegularityDetectResultWithPositionsEqualityComparer : IEqualityComparer<RegularityDetectResultWithPositions>
        {
            public bool Equals(RegularityDetectResultWithPositions x, RegularityDetectResultWithPositions y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Equals(y);
            }

            public int GetHashCode(RegularityDetectResultWithPositions obj)
            {
                return obj.GetHashCode();
            }
        }

        public new static IEqualityComparer<RegularityDetectResultWithPositions> Comparer { get; } = new RegularityDetectResultWithPositionsEqualityComparer();
        
        #endregion

        public byte[] Positions { get; set; }
    }
}