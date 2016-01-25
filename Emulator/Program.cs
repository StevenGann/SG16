using SG16;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();

            core.LoadROM(0, "test.rom");


            while(core.PC.ToInt() < core.RAM.Data.Length)
            {
                long time = core.Tick();
                if (time >= 1)
                {
                    Console.Write(" PC=" + core.PC.ToInt().ToString("D2") + " - ");
                    Console.Write(core.Message);
                    Console.Write(" - Took " + Convert.ToString(time) + "ms");
                    Console.Write("\n");
                }
                
            }

            Console.ReadLine();
        }
    }
}
