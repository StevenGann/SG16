using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    class Instruction
    {
        public byte Opcode = 0x00;
        public byte[] Argument1 = new byte[3];
        public byte[] Argument2 = new byte[3];

        public Instruction()
        {
            Opcode = 0x00;
            Argument1[0] = 0x00;
            Argument1[1] = 0x00;
            Argument1[2] = 0x00;
            Argument2[0] = 0x00;
            Argument2[1] = 0x00;
            Argument2[2] = 0x00;
        }

        public Instruction(byte[] input)
        {
            Opcode = input[0];
            Argument1[0] = input[1];
            Argument1[1] = input[2];
            Argument1[2] = input[3];
            Argument2[0] = input[4];
            Argument2[1] = input[5];
            Argument2[2] = input[6];
        }

        public Instruction(byte[] input, int offset)
        {
            Opcode = input[0 + offset];
            Argument1[0] = input[1 + offset];
            Argument1[1] = input[2 + offset];
            Argument1[2] = input[3 + offset];
            Argument2[0] = input[4 + offset];
            Argument2[1] = input[5 + offset];
            Argument2[2] = input[6 + offset];
        }

        public byte[] ToArray()
        {
            byte[] result = new byte[8];

            result[0] = Opcode;
            result[1] = Argument1[0];
            result[2] = Argument1[1];
            result[3] = Argument1[2];
            result[4] = Argument2[0];
            result[5] = Argument2[1];
            result[6] = Argument2[2];
            result[7] = 0xFF;

            return result;
        }

        override public string ToString()
        {
            string result = "";
            AssemblyTable table = new AssemblyTable();
            result += table.GetInstruction(Opcode);
            result += "," + ByteArrayToString(Argument1);
            result += "," + ByteArrayToString(Argument2);
            return result;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.Append(' ');
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString().ToUpper();
        }
    }
}
