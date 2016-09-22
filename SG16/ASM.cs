using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class ASM
    {
        public void Assemble(string _input, string _output)
        {
            //Split code file into lines
            string[] inputArray = _input.Split('\n');
            List<string> instructions = new List<string>(inputArray);

            //Remove comments and blank lines
            for (int i = 0; i < instructions.Count; i++)
            {
                //Trim leading whitespace
                instructions[i] = instructions[i].TrimStart(null);

                if (String.IsNullOrWhiteSpace(instructions[i])) //Check for blank line
                {
                    instructions.RemoveAt(i);
                    i--;
                }
                else if (instructions[i][0] == '#') //Check for comment
                {
                    instructions.RemoveAt(i);
                    i--;
                }
            }

            //Convert each Assembly instruction to machine code
            List<byte> program = new List<byte>();
            foreach (string line in instructions)
            {
                Console.WriteLine("--------------");
                Console.WriteLine(line);
                Console.WriteLine(ByteArrayToString(StringToMachineCode(line)));
                program.AddRange(StringToMachineCode(line));
            }
            Console.WriteLine("==============");
            Console.WriteLine(ByteArrayToString(program.ToArray()));

            using (BinaryWriter writer = new BinaryWriter(File.Open(_output, FileMode.Create)))
            {
                foreach (byte b in program)
                {
                    writer.Write(b);
                }
            }

            Console.ReadLine();
        }

        private byte[] StringToMachineCode(string _input)
        {
            byte[] instruction = new byte[8];

            instruction[0] = 0x00; //NULL opcode
            instruction[1] = 0x01; //Reference type Literal
            instruction[2] = 0x00; //Data upper byte
            instruction[3] = 0x00; //Data lower byte
            instruction[4] = 0x01; //Reference type Literal
            instruction[5] = 0x00; //Data upper byte
            instruction[6] = 0x00; //Data lower byte
            instruction[7] = 0xFF; //RESERVED byte

            string[] tokenArray = _input.Split(' ');
            List<string> tokens = new List<string>(tokenArray);

            for (int i = 0; i < tokens.Count; i++)
            {
                tokens[i] = tokens[i].Trim(null);
                //tokens[i] = tokens[i].ToUpper();
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (String.IsNullOrWhiteSpace(tokens[i])) //Check for blank line
                {
                    tokens.RemoveAt(i);
                    i--;
                }
                if (tokens[i][0] == '#')//Check for inline comment
                {
                    tokens.RemoveRange(i, tokens.Count - i);
                }
            }
            AssemblyTable table = new AssemblyTable();
            instruction[0] = table.GetOpcode(tokens[0]);
            if (tokens.Count >= 2)
            {
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                if (tokens.Count >= 3)
                {
                    b = ParseArgument(tokens[2]);
                    instruction[4] = b[0];
                    instruction[5] = b[1];
                    instruction[6] = b[2];
                }
            }
            return instruction;
        }

        public byte[] ParseArgument(string _input)
        {
            byte[] result = new byte[3];
            result[0] = 0x01;//Failsafe value, 0x0000 literal.
            result[1] = 0x00;
            result[2] = 0x00;

            if (_input[0] == 's')//ASCII literal
            {
                result[0] = 0x01;
                throw new NotImplementedException();
            }
            else if (_input[0] == 'o')//Octal literal
            {
                result[0] = 0x01;
                throw new NotImplementedException();
            }
            else if (_input[0] == 'x')//Hex literal
            {
                result[0] = 0x01;
                string raw = _input.Remove(0, 1);
                byte[] data = StringToByteArray(raw);
                result[1] = data[0];
                result[2] = data[1];
            }
            else if (_input[0] == 'd')//Decimal literal
            {
                result[0] = 0x01;
                throw new NotImplementedException();
            }
            else if (_input[0] == 'b')//Binary literal
            {
                result[0] = 0x01;
                throw new NotImplementedException();
            }
            else if (_input[0] == '@')//Absolute address
            {
                result[0] = 0x02;
                string raw = _input.Remove(0, 1);
                byte[] data = StringToByteArray(raw);
                result[1] = data[0];
                result[2] = data[1];
            }
            else if (_input[0] == '$')//Indirect address
            {
                result[0] = 0x03;
                throw new NotImplementedException();
            }
            else //Must be a register
            {
                result[0] = 0x00;
                if (_input == "PC")
                {
                    result[2] = 0x00;
                }
                else if (_input == "STAT")
                {
                    result[2] = 0x01;
                }
                else if (_input == "SUBR")
                {
                    result[2] = 0x02;
                }
                else if (_input == "PSTR")
                {
                    result[2] = 0x03;
                }
                else if (_input == "PEND")
                {
                    result[2] = 0x04;
                }
                else if (_input == "RAND")
                {
                    result[2] = 0x05;
                }
                else if (_input == "RREF")
                {
                    result[2] = 0x06;
                }
                else if (_input == "USR0")
                {
                    result[2] = 0xF0;
                }
                else if (_input == "USR1")
                {
                    result[2] = 0xF1;
                }
                else if (_input == "USR2")
                {
                    result[2] = 0xF2;
                }
                else if (_input == "USR3")
                {
                    result[2] = 0xF3;
                }
                else if (_input == "USR4")
                {
                    result[2] = 0xF4;
                }
                else if (_input == "USR5")
                {
                    result[2] = 0xF5;
                }
                else if (_input == "USR6")
                {
                    result[2] = 0xF6;
                }
                else if (_input == "USR7")
                {
                    result[2] = 0xF7;
                }
                else if (_input == "USR8")
                {
                    result[2] = 0xF8;
                }
                else if (_input == "USR9")
                {
                    result[2] = 0xF9;
                }
                else if (_input == "USRA")
                {
                    result[2] = 0xFA;
                }
                else if (_input == "USRB")
                {
                    result[2] = 0xFB;
                }
                else if (_input == "USRC")
                {
                    result[2] = 0xFC;
                }
                else if (_input == "USRD")
                {
                    result[2] = 0xFD;
                }
                else if (_input == "USRE")
                {
                    result[2] = 0xFE;
                }
                else if (_input == "USRF")
                {
                    result[2] = 0xFF;
                }
            }

            return result;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            int i = 0;
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
                if (ba.Length <= 8 && i == 0) { hex.Append(" "); }
                if (ba.Length <= 8 && i == 1) { hex.Append(" "); }
                if (ba.Length <= 8 && i == 3) { hex.Append(" "); }
                if (ba.Length <= 8 && i == 4) { hex.Append(" "); }
                if (ba.Length <= 8 && i == 6) { hex.Append(" "); }
                i++;
            }
            return hex.ToString().ToUpper();
        }

        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}