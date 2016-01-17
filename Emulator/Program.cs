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
                Console.Write(" PC=" + core.PC.ToInt().ToString("D2") + " - ");
                long time = core.Tick();
                //Console.Write(" PSTR=" + core.PSTR.ToInt());
                Console.WriteLine(" - Took " + Convert.ToString(time) + "ms");
            }

            Console.ReadLine();
        }
    }
}
