using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assembler;
using SG16;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            ASM Assembler = new ASM();
            string input = System.IO.File.ReadAllText("test.asm");
            Console.WriteLine(input);
            Assembler.Assemble(input);
        }
    }
}
