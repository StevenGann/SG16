using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class Memory
    {
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

        public byte[] Get16(byte[] address)
        {
            byte[] result = new byte[2];
            int index = BitConverter.ToUInt16(address, 0);
            result[0] = Data[index];
            result[1] = Data[index + 1];
            return result;
        }

        public void Set16(byte[] address, byte[] value)
        {
            byte[] indexBytes = new byte[2];
            indexBytes[0] = address[2];
            indexBytes[1] = address[1];
            int index = BitConverter.ToUInt16(indexBytes, 0);
            Data[index] = value[0];
            Data[index + 1] = value[1];
        }

    }
}
