Rx.Marble

Reactive MIDI Pipeline Builder

Rx.Marble is a proof-of-concept application built with C# and WPF, demonstrating a versatile use case for Rx.NET in creating dynamic and interactive systems. The core idea is to provide a visual, drag-and-drop interface for users to combine modules and construct reactive MIDI note sequences that can be modified on the fly, enabling the creation of intricate rhythmic patterns.
Current Features

While still in its early stages and primarily serving as a proof of concept, Rx.Marble currently offers:

    Drag-and-Drop Module System: Visually arrange and connect modules to build your MIDI pipeline.
    Metronome Module: Establishes the rhythmic pulse for your sequences.
    Simple Note Operator: Set a specific MIDI note and octave within your sequence.
    Channel Operator: Direct your MIDI output to a chosen channel.

Vision

The long-term goal for Rx.Marble is to evolve into a comprehensive MIDI pipeline builder, incorporating a wider array of modules such as:

    NoteSelectorOperator
    TransposeOperator
    ChannelOperator
    DelayOperator
    ChordSelector with Arpeggio capabilities

...and many more, allowing for highly complex and customizable musical compositions through a visually intuitive interface.
Important Note on Stability

Please be aware that Rx.Marble is currently in a highly unstable state. Changing notes and tempo values can lead to unexpected crashes. This project is intended to showcase the potential of Rx.NET for complex, real-time interactions rather than being a production-ready application.
Technologies Used

    C#
    WPF
    Rx.NET
    MVVMCommunityPackage
    Googles MaterialDesign

Getting Started

To explore Rx.Marble, you'll need to set up a C# WPF project.
Prerequisites

    Visual Studio: Community, Professional, or Enterprise edition (2019 or later recommended).
    .NET SDK: The appropriate version for your Visual Studio installation (e.g., .NET 6, .NET 7, or .NET 8).

Basic Setup Instructions

    Clone the Repository:
    Bash

git clone [Your Repository URL Here]
cd Rx.Marble

(Replace [Your Repository URL Here] with the actual URL of your Git repository.)

Open in Visual Studio:

    Open Visual Studio.
    Click on File > Open > Project/Solution...
    Navigate to the cloned Rx.Marble folder and select the .sln (solution) file.

Restore NuGet Packages:

    Visual Studio should automatically prompt you to restore any missing NuGet packages (including Rx.NET). If not, you can right-click on your solution in the Solution Explorer and select Restore NuGet Packages.

Build the Project:

    From the Visual Studio menu, go to Build > Build Solution, or simply press F6.

Run the Application:

    Press F5 or click the green "Play" button in Visual Studio to start the application.
