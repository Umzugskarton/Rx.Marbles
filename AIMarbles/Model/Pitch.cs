using AIMarbles.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Model
{
    public struct Pitch
    {
        public NoteName Note { get; }
        public int Octave { get; }

        public Pitch(NoteName note, int octave)
        {
            Note = note;
            Octave = octave;
        }

        public override string ToString()
        {
            return $"{Note}{Octave}"; // e.g., C4, GSharp3
        }

        public override bool Equals(object obj)
        {
            return obj is Pitch other &&
                   Note == other.Note &&
                   Octave == other.Octave;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Note, Octave);
        }

        // Optional: Method to get the MIDI note number (useful for calculations)
        // Assuming C0 = 12 (or 0 if you start your MIDI range there)
        // MIDI Pitch Number for C4 (Octave 4, NoteName.C=0) would be 60.
        // So, C0 = (0 * 12) + 0 = 0 (if C0 is MIDI 0)
        // Or C0 = 12 if you use the convention where C-1 is MIDI 0
        public int GetMidiNoteNumber(int c0MidiNumber = 0) // Default C0 is MIDI 12
        {
            return c0MidiNumber + (Octave * 12) + (int)Note;
        }

        // Example of arithmetic: getting the next semitone
        public Pitch AddSemitones(int semitones)
        {
            int currentMidi = GetMidiNoteNumber();
            int newMidi = currentMidi + semitones;

            // Convert back from MIDI note number
            // Assuming C0MidiNumber = 12, then C0's internal pitch class is 0.
            // newMidi - C0MidiNumber gives the absolute note index from C0
            if (newMidi < 0) throw new ArgumentOutOfRangeException(nameof(semitones), "Resulting pitch is below C0.");

            int newOctave = newMidi / 12;
            NoteName newNote = (NoteName)(newMidi % 12);

            return new Pitch(newNote, newOctave);
        }
    }

}
