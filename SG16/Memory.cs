using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class Memory
    {
        //I don't like having an array this gigantic.
        //I've wrapped it in a class so that I can later make it more intelligent.
        //Perhaps I could store the data in a file?
        public byte[] Data = new byte[65536];
        public int Size = 65536;

        public Memory()
        {
            for (int i = 0; i < 65536; i++)
            {
                Data[i] = 0x00;
            }
        }



        public byte this[int index]
        {
            get
            {
                return Data[index];
            }

            set
            {
                Data[index] = value;
            }
        }

        public byte Get(byte upper, byte lower)
        {
            return Data[Convert.ToInt16(upper) + Convert.ToInt16(lower)];
        }
    }
}
