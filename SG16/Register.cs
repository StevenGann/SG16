using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class Register
    {
        public byte UpperByte = 0x00;
        public byte LowerByte = 0x00;

        public void Increment(uint d)
        {
            if ((d + (uint)LowerByte) <= 255)
            {
                LowerByte = (byte)(d + (uint)LowerByte);
            }
            else if ((d + (uint)LowerByte) > 255)
            {
                LowerByte = 0x00;
                UpperByte = (byte)((d + (uint)LowerByte - 255) + (uint)UpperByte);
            }
        }

        public static explicit operator Register(byte b)
        {
            Register result = new Register();
            result.LowerByte = b;
            return result;
        }

        public static explicit operator Register(byte[] b)
        {
            Register result = new Register();
            result.LowerByte = b[0];
            if (b.Length > 1) { result.UpperByte = b[1]; }
            return result;
        }

        public static explicit operator Register(UInt16 b)
        {
            Register result = new Register();

            if (b <= 0xFF)
            {
                result.LowerByte = Convert.ToByte(b);
            }
            else
            {
                result.LowerByte = 0xFF;
                result.UpperByte = Convert.ToByte(b - Convert.ToUInt16(0xFF));
            }

            return result;
        }

        public int ToInt()
        {
            return (int)(Convert.ToUInt32(UpperByte) + Convert.ToUInt32(LowerByte));
        }

        new public string ToString()
        {
            byte[] data = new byte[2];
            data[0] = UpperByte;
            data[1] = LowerByte;
            return ASM.ByteArrayToString(data);
        }

        public static bool GetBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        public static byte SetBit(byte b, int pos)
        {
            if (pos < 0 || pos > 7)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-7.");

            return (byte)(b | (1 << pos));
        }

        public static byte UnsetBit(byte b, int pos)
        {
            if (pos < 0 || pos > 7)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-7.");

            return (byte)(b & ~(1 << pos));
        }

        public static byte WriteBit(byte b, int pos, bool value)
        {
            if (value == true)
            {
                return SetBit(b, pos);
            }
            else
            {
                return UnsetBit(b, pos);
            }
        }
    }
}