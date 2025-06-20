using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIMarbles.Core.Interface;
using AIMarbles.Model;
using NAudio.Midi;

namespace AIMarbles.Core.Service
{
    public class MIDIOutputService : IMidiOutputService
    {
        MidiOut? _midiOut;

        List<String> midiOutDevices = new List<string>();

        public MIDIOutputService()
        {
            while (MidiOut.NumberOfDevices == 0)
            {
                Console.WriteLine("No MIDI output devices found. Please connect a MIDI device.");
                System.Threading.Thread.Sleep(1000); // Wait before retrying
            }

            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                string name = MidiOut.DeviceInfo(i).ProductName;
                midiOutDevices.Add(name);

                Console.WriteLine($"Registered MIDIout:{name} ::: Index: {i}");
            }

            try
            {
                _midiOut = new MidiOut(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MIDI output: {ex.Message}");
            }
        }

        public void SendToMidiOut(MIDIMarble marble) 
        {
            var midiMessage = new NoteOnEvent(
                0,
                marble.Channel,
                marble.Pitch.GetMidiNoteNumber(),
                marble.Velocity,
                marble.DurationMs
            ).GetAsShortMessage();

            _midiOut?.Send(midiMessage);
        }

        public void Dispose()
        {
            _midiOut?.Dispose();
            _midiOut = null;
        }
    }
}
