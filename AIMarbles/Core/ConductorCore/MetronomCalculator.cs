using AIMarbles.Core.Helper;
using AIMarbles.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.ConductorCore
{
    public class MetronomeCalculator
    {
        /// <summary>
        /// Calculates the interval in milliseconds for each beat, based on BPM and the time signature's denominator.
        /// </summary>
        /// <param name="bpm">Beats Per Minute.</param>
        /// <param name="timeSignature">The selected TimeSignature enum value.</param>
        /// <returns>The interval as a TimeSpan in milliseconds between each beat.</returns>
        public static TimeSpan CalculateBeatIntervalMs(double bpm, TimeSignature timeSignature)
        {
            if (bpm <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bpm), "BPM must be greater than zero.");
            }

            int denominator = TimeSignatureHelper.GetDenominator(timeSignature);

            // Calculate milliseconds for a whole note at this BPM.
            // A "beat" is defined by the denominator. If BPM is 120, that means 120 *quarter notes* per minute.
            // So, 60000 / BPM gives the ms for ONE quarter note.
            // We need to adjust this based on the denominator.
            // Example:
            // If denominator is 4 (quarter note): (60000 / BPM) * (4 / 4) = 60000 / BPM
            // If denominator is 8 (eighth note): (60000 / BPM) * (4 / 8) = (60000 / BPM) * 0.5
            // So, the formula is: (60000 / BPM) * (4.0 / denominator)

            double millisecondsPerQuarterNote = 60000.0 / bpm;
            double beatIntervalMs = millisecondsPerQuarterNote * (4.0 / denominator);

            return TimeSpan.FromMilliseconds(beatIntervalMs);
        }

        /// <summary>
        /// Calculates the interval in milliseconds for a specific note value at a given BPM.
        /// (This is the more general calculation from our earlier discussion, useful if you need to subdivide beats).
        /// </summary>
        /// <param name="bpm">Beats Per Minute.</param>
        /// <param name="noteValueFactor">The note value as a fraction of a whole note (e.g., 1 for whole, 0.5 for half, 0.25 for quarter).</param>
        /// <returns>The interval in milliseconds for that note value.</returns>
        public static double CalculateNoteDurationMs(double bpm, double noteValueFactor)
        {
            if (bpm <= 0) throw new ArgumentOutOfRangeException(nameof(bpm), "BPM must be greater than zero.");
            if (noteValueFactor <= 0) throw new ArgumentOutOfRangeException(nameof(noteValueFactor), "Note value factor must be greater than zero.");

            // Calculate milliseconds per beat (assuming a quarter note as the base beat for BPM)
            double millisecondsPerBeat = 60000.0 / bpm;

            // Adjust based on the note value factor relative to a quarter note (1/4 of a whole note)
            // A whole note is 4 quarter notes. So, if factor is 1 (whole note), it's 4 * millisecondsPerBeat
            // If factor is 0.25 (quarter note), it's 1 * millisecondsPerBeat
            return millisecondsPerBeat * (noteValueFactor * 4); // Factor of 4 because BPM is usually quarter-note based
        }
    }
}
