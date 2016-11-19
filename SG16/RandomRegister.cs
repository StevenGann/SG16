using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class RandomRegister : Register
    {
        private byte upperByte = 0x00;
        private byte lowerByte = 0x00;
        private Random RNG = new Random();

        public new byte UpperByte
        {
            get
            {
                upperByte = (byte)RNG.Next();
                return upperByte;
            }
            set
            {
                upperByte = (byte)RNG.Next();
            }
        }

        public new byte LowerByte
        {
            get
            {
                lowerByte = (byte)RNG.Next();
                return lowerByte;
            }
            set
            {
                lowerByte = (byte)RNG.Next();
            }
        }

        public static explicit operator RandomRegister(byte b)
        {
            RandomRegister result = new RandomRegister();
            Random rng = new Random();
            result.upperByte = (byte)rng.Next();
            result.lowerByte = (byte)rng.Next();
            return result;
        }

        public static explicit operator RandomRegister(byte[] b)
        {
            RandomRegister result = new RandomRegister();
            Random rng = new Random();
            result.upperByte = (byte)rng.Next();
            result.lowerByte = (byte)rng.Next();
            return result;
        }

        public static explicit operator RandomRegister(UInt16 b)
        {
            RandomRegister result = new RandomRegister();
            Random rng = new Random();
            result.upperByte = (byte)rng.Next();
            result.lowerByte = (byte)rng.Next();
            return result;
        }

        new public int ToInt()
        {
            return (int)(Convert.ToUInt32(upperByte) + Convert.ToUInt32(lowerByte));
        }

        new public string ToString()
        {
            byte[] data = new byte[2];
            data[0] = upperByte;
            data[1] = lowerByte;
            return ASM.ByteArrayToString(data);
        }
    }
}