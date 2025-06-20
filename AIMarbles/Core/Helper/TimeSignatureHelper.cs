using AIMarbles.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Helper
{
    public static class TimeSignatureHelper
    {

        public static int GetDenominator(TimeSignature ts)
        {
            return (int)ts % 100;
        }

        public static int GetNumerator(TimeSignature ts)
        {
            return (int)ts / 100;
        }

    }
}
