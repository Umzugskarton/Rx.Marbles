using AIMarbles.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Model
{
    public class MIDIMarble
    {
        public Pitch Pitch { get; set; }
        public int Velocity { get; set; }
        public int DurationMs { get; set; }
        public int Channel { get; set; } = 0; // Default channel
        
        public DateTime Timestamp { get; set; } = DateTime.Now; // Example: for timing/delays

        public MIDIMarble Clone() // Useful for non-mutating operations
        {
            return new MIDIMarble
            {
                Pitch = this.Pitch,
                Velocity = this.Velocity,
                Channel = this.Channel,
                Timestamp = this.Timestamp
            };
        }

        public override string ToString()
        {
            return $"MIDIMarble [NoteName: {Pitch.Note}, Octave: {Pitch.Octave}, Vel: {Velocity}, Ch: {Channel}]";
        }
    }
}
