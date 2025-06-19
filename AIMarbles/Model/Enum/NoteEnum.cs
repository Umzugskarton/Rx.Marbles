namespace AIMarbles.MusicTheory
{
    /// <summary>
    /// Represents the 12 chromatic notes in an octave.
    /// The underlying integer value corresponds to its relative MIDI note number (0-11),
    /// where C is 0 and B is 11.
    /// </summary>
    public enum NoteName
    {
        // C - 0
        C = 0,
        CSharp = 1, // Also D-flat (Db)
        D = 2,
        DSharp = 3, // Also E-flat (Eb)
        E = 4,
        F = 5,
        FSharp = 6, // Also G-flat (Gb)
        G = 7,
        GSharp = 8, // Also A-flat (Ab)
        A = 9,
        ASharp = 10, // Also B-flat (Bb)
        B = 11
    }

    /// <summary>
    /// Provides utility methods for working with the NoteName enum,
    /// including string parsing and MIDI note conversions.
    /// </summary>
    public static class NoteExtensions
    {
        // A static dictionary for efficient string-to-NoteName mapping.
        // It includes both sharp and flat common names, mapping them to the canonical sharp enum values.
        private static readonly IReadOnlyDictionary<string, NoteName> _noteStringMap =
            new Dictionary<string, NoteName>(StringComparer.OrdinalIgnoreCase)
            {
                { "C", NoteName.C },
                { "C#", NoteName.CSharp },
                { "Db", NoteName.CSharp }, // Db maps to C#
                { "D", NoteName.D },
                { "D#", NoteName.DSharp },
                { "Eb", NoteName.DSharp }, // Eb maps to D#
                { "E", NoteName.E },
                { "F", NoteName.F },
                { "F#", NoteName.FSharp },
                { "Gb", NoteName.FSharp }, // Gb maps to F#
                { "G", NoteName.G },
                { "G#", NoteName.GSharp },
                { "Ab", NoteName.GSharp }, // Ab maps to G#
                { "A", NoteName.A },
                { "A#", NoteName.ASharp },
                { "Bb", NoteName.ASharp }, // Bb maps to A#
                { "B", NoteName.B }
            };

        /// <summary>
        /// Attempts to parse a string representation of a musical note into its corresponding NoteName enum value.
        /// Handles common sharp (#) and flat (b) notations.
        /// </summary>
        /// <param name="noteString">The string representation of the note (e.g., "C", "F#", "Bb", "db").</param>
        /// <param name="note">When this method returns, contains the NoteName enum value if the conversion succeeded, or the default value if the conversion failed.</param>
        /// <returns>True if the noteString was converted successfully; otherwise, false.</returns>
        public static bool TryParseNote(string noteString, out NoteName note)
        {
            if (string.IsNullOrWhiteSpace(noteString))
            {
                note = default;
                return false;
            }

            // Attempt to find the note in our predefined map
            return _noteStringMap.TryGetValue(noteString, out note);
        }

        /// <summary>
        /// Parses a string representation of a musical note into its corresponding NoteName enum value.
        /// Throws an ArgumentException if the string is not a valid note.
        /// </summary>
        /// <param name="noteString">The string representation of the note (e.g., "C", "F#", "Bb").</param>
        /// <returns>The NoteName enum value.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided string is not a recognized musical note.</exception>
        public static NoteName ParseNote(string noteString)
        {
            if (TryParseNote(noteString, out NoteName note))
            {
                return note;
            }
            throw new ArgumentException($"'{noteString}' is not a recognized musical note.", nameof(noteString));
        }

        /// <summary>
        /// Converts a NoteName enum value to its relative MIDI note number (0-11).
        /// This is simply the underlying integer value of the enum.
        /// </summary>
        /// <param name="note">The NoteName enum value.</param>
        /// <returns>The relative MIDI note number (0-11).</returns>
        public static int ToMidiRelativeNote(this NoteName note)
        {
            return (int)note;
        }

        /// <summary>
        /// Converts a NoteName enum value and an octave number to an absolute MIDI note number (0-127).
        /// MIDI note 0 is C-1, MIDI note 60 is C4 (Middle C).
        /// </summary>
        /// <param name="note">The NoteName enum value (e.g., C, FSharp).</param>
        /// <param name="octave">The octave number (e.g., 4 for C4, 3 for C3). MIDI C0 is octave 0, C4 is middle C.</param>
        /// <returns>The absolute MIDI note number (0-127).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the resulting MIDI note is outside the 0-127 range.</exception>
        public static int ToAbsoluteMidiNote(this NoteName note, int octave)
        {
            // MIDI C0 is note 0. Octaves are typically numbered from -1 (C-1) to 9 (G9).
            // Middle C (C4) is MIDI note 60.
            // Our relative note (0-11) is based on C.
            // MIDI note = (octave + 1) * 12 + relativeNoteValue
            // (Since C0 is MIDI 0, C-1 would technically be MIDI -12, so we add 1 to octave)
            // Example: C4 => (4 + 1) * 12 + 0 = 60
            // Example: A4 => (4 + 1) * 12 + 9 = 69
            // Example: C-1 => (-1 + 1) * 12 + 0 = 0 (This is MIDI note 0)

            int absoluteNote = (octave + 1) * 12 + note.ToMidiRelativeNote();

            if (absoluteNote < 0 || absoluteNote > 127)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(octave),
                    $"The combination of note {note} and octave {octave} results in MIDI note {absoluteNote}, which is outside the valid MIDI range (0-127)."
                );
            }

            return absoluteNote;
        }

        /// <summary>
        /// Converts an absolute MIDI note number (0-127) back to a NoteName enum and its octave.
        /// </summary>
        /// <param name="midiNote">The absolute MIDI note number (0-127).</param>
        /// <returns>A tuple containing the NoteName enum and its corresponding octave.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the MIDI note is outside the 0-127 range.</exception>
        public static (NoteName note, int octave) FromAbsoluteMidiNote(int midiNote)
        {
            if (midiNote < 0 || midiNote > 127)
            {
                throw new ArgumentOutOfRangeException(nameof(midiNote), "MIDI note must be between 0 and 127.");
            }

            int relativeNoteValue = midiNote % 12;
            int octave = (midiNote / 12) - 1; // Subtract 1 because MIDI C0 is octave 0, which corresponds to 0 / 12 = 0

            return ((NoteName)relativeNoteValue, octave);
        }
    }

}
