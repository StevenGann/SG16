using SG16;
using System;
using System.Collections.Generic;
using System.IO;

namespace Disassembler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string inputPath = "disassembler_test.rom";

            if (args.Length >= 1)
            {
                if (File.Exists(args[0]) && Path.GetExtension(args[0]) == ".rom")
                {
                    inputPath = args[0];
                    Console.WriteLine(inputPath);
                }
            }

            if (File.Exists(inputPath))
            {
                ASM Disassembler = new ASM();
                BinaryReader reader = new BinaryReader(File.Open(inputPath, FileMode.Open));

                List<byte> bytesList = new List<byte>();
                bool reading = true;
                while (reading)
                {
                    try { bytesList.Add(reader.ReadByte()); }
                    catch { reading = false; }
                }

                string code = Disassembler.Disassemble(bytesList.ToArray());

                File.WriteAllText(Path.GetFileNameWithoutExtension(inputPath) + ".asm", code);
            }
            else
            {
                if (inputPath != "")
                {
                    Console.WriteLine("Could not find file:");
                    Console.WriteLine(inputPath);
                }
                else
                {
                    Console.WriteLine("SG16 Disassembler");
                    Console.WriteLine("Usage: Disassembler.exe <input file>.rom");
                    Console.WriteLine("Drag-and-drop is also supported.");
                }
                Console.ReadLine();
            }
        }
    }
}