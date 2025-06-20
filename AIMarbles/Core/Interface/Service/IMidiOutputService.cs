using AIMarbles.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Interface
{
    public interface IMidiOutputService
    {
        void SendToMidiOut(MIDIMarble marble);

        public void Dispose();
    }
}
