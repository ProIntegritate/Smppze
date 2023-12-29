# Snooze
A better replacement to Windows Timeout command

I sit at my computer. A lot. And i sometimes burn food and miss other things i should do.  So i wrote a better timer that has beeps when starting/stopping, can calculate time i.e. 1hour + 30 minutes - 15 seconds.

Snooze is a console application that can be used like Timeout, but has more fine grain control over how long you can wait, and not just in seconds.

The sourcecode is available in this repo, just copy/paste it into visual studio and build (Currently mine is compiled with Framework 4.8, but should work with .NET 5+) and then build it into an executable.  I even did a Linux port at one point in .NET 6, but there were some problems with Console.Top and there were also no Console.Beep that worked out of the box (as most Linux flavour doesn't have sound drivers installed by default).

You can get help by just starting the compiled binary with no arguments: (or -h for help)<br />
<img src="https://github.com/ProIntegritate/Snooze/blob/main/x2.jpg?raw=true">

Here is what it looks like when the timer is running.<br />
<img src="https://github.com/ProIntegritate/Snooze/blob/main/x.gif?raw=true">
