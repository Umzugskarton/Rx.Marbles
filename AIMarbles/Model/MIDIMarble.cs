using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Model
{
    class MIDIMarble
    {
        public int NoteNumber { get; set; }
        public int Velocity { get; set; }
        public int DurationMs { get; set; }
        public int Channel { get; set; } = 0; // Default channel
    }
}
