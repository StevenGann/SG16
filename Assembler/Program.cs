using SG16;
using System;
using System.IO;

namespace Assembler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string inputPath = "assembler_test.asm";

            if (args.Length >= 1)
            {
                if (File.Exists(args[0]) && Path.GetExtension(args[0]) == ".asm")
                {
                    inputPath = args[0];
                    Console.WriteLine(inputPath);
                }
            }

            if (File.Exists(inputPath))
            {
                ASM Assembler = new ASM();
                string input = System.IO.File.ReadAllText(inputPath);
                Console.WriteLine(input);
                Assembler.Assemble(input, Path.GetFileNameWithoutExtension(inputPath) + ".rom");
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
                    Console.WriteLine("SG16 Assembler");
                    Console.WriteLine("Usage: Assembler.exe <input file>.asm");
                    Console.WriteLine("Drag-and-drop is also supported.");
                }
                Console.ReadLine();
            }
        }
    }
}