using SG16;
using System;
using System.IO;

//Simplest application of the emulator.
//Example of how little support is needed to run the core emulator.

namespace Emulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                bool fastMode = false;
                if (args.Length >= 2) { fastMode = (args[1] == "-fast"); }

                if (File.Exists(args[0]) && Path.GetExtension(args[0]) == ".rom")
                {
                    Core core = new Core();
                    Console.WriteLine("Loading ROM file: " + args[0]);
                    core.LoadROM(0, args[0]);
                    Console.WriteLine("Done!");
                    long time = 0;
                    core.Initialize();
                    while (core.PC.ToInt() < core.RAM.Data.Length)
                    {
                        time = core.Tick();

                        if (!fastMode && time >= 1)
                        {
                            Console.Write(" PC=" + core.PC.ToInt().ToString("D2") + " - ");
                            Console.Write(core.Message);
                            Console.Write(" - Took " + Convert.ToString(time) + "ms");
                            Console.Write("\n");
                        }

                        while (core.TXD0buffer.Count > 0)
                        {
                            Console.Write((char)core.TXD0buffer[0]);
                            core.TXD0buffer.RemoveAt(0);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid or missing file:");
                    Console.WriteLine(args[0]);
                }
            }
            else
            {
                Console.WriteLine("CLI SG16 Emulator");
                Console.WriteLine("Usage: Emulator.exe <input file>.rom [-fast]");
                Console.WriteLine("Optional \"-fast\" parameter silences console output for improved emulation speed.");
            }

            Console.ReadLine();
        }
    }
}