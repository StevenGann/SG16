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


            for (int i = 0; i < 10; i++)
            {
                long time = core.Tick();
                Console.Write(" PC=" + core.PC.ToInt());
                //Console.Write(" PSTR=" + core.PSTR.ToInt());
                Console.WriteLine("\nTook " + Convert.ToString(time) + "ms");
            }

            Console.ReadLine();
        }
    }
}
