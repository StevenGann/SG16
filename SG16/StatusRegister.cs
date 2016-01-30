using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class StatusRegister : Register
    {
        public bool Z
        {
            get { return GetBit(LowerByte, 0); }
            set { WriteBit(LowerByte, 0, value); }
        }
        public bool C
        {
            get { return GetBit(LowerByte, 1); }
            set { WriteBit(LowerByte, 1, value); }
        }
        public bool N
        {
            get { return GetBit(LowerByte, 2); }
            set { WriteBit(LowerByte, 2, value); }
        }
        public bool O
        {
            get { return GetBit(LowerByte, 3); }
            set { WriteBit(LowerByte, 3, value); }
        }
        public bool P
        {
            get { return GetBit(LowerByte, 4); }
            set { WriteBit(LowerByte, 4, value); }
        }
        public bool E
        {
            get { return GetBit(LowerByte, 5); }
            set { WriteBit(LowerByte, 5, value); }
        }
        public bool L
        {
            get { return GetBit(LowerByte, 6); }
            set { WriteBit(LowerByte, 6, value); }
        }
        public bool G
        {
            get { return GetBit(LowerByte, 7); }
            set { WriteBit(LowerByte, 7, value); }
        }

        public static explicit operator StatusRegister(byte b)
        {
            StatusRegister result = new StatusRegister();
            result.LowerByte = b;
            return result;
        }

        public static explicit operator StatusRegister(byte[] b)
        {
            StatusRegister result = new StatusRegister();
            result.LowerByte = b[0];
            if (b.Length > 1) { result.UpperByte = b[1]; }
            return result;
        }

        public static explicit operator StatusRegister(UInt16 b)
        {
            StatusRegister result = new StatusRegister();

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

        new public string ToString()
        {
            string lower = Convert.ToString(LowerByte, 2).PadLeft(8, '0');
            string upper = Convert.ToString(UpperByte, 2).PadLeft(8, '0');
            return upper + lower;
        }
    }
}
