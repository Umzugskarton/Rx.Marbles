using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Model.Enum
{
    public enum TimeSignature
    {
        // Common Time Signatures
        [Description("4/4")]
        CommonTime_4_4 = 404, // Represents 4 beats per measure, quarter note gets the beat

        [Description("2/2")]
        CutTime_2_2 = 202, // Represents 2 beats per measure, half note gets the beat

        [Description("3/4")]
        ThreeFour = 304, // 3 beats per measure, quarter note gets the beat

        [Description("6/8")]
        SixEight = 608, // 6 beats per measure, eighth note gets the beat (often felt in 2)

        [Description("9/8")]
        NineEight = 908, // 9 beats per measure, eighth note gets the beat (often felt in 3)

        [Description("12/8")]
        TwelveEight = 1208, // 12 beats per measure, eighth note gets the beat (often felt in 4)

        // Less Common Time Signatures
        [Description("2/4")]
        TwoFour = 204, // 2 beats per measure, quarter note gets the beat

        [Description("5/4")]
        FiveFour = 504, // 5 beats per measure, quarter note gets the beat

        [Description("7/4")]
        SevenFour = 704, // 7 beats per measure, quarter note gets the beat

        [Description("5/8")]
        FiveEight = 508, // 5 beats per measure, eighth note gets the beat

        [Description("7/8")]
        SevenEight = 708 // 7 beats per measure, eighth note gets the beat
    }
}
