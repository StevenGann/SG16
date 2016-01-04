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
        public void Assemble(string _input)
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

            using (BinaryWriter writer = new BinaryWriter(File.Open("output.rom", FileMode.Create)))
            {
                foreach(byte b in program)
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
            }

            #region Opcodes
            if (tokens[0] == "NULL")
            {
                instruction[0] = 0x00;
            }
            else if (tokens[0] == "START")
            {
                instruction[0] = 0x01;
            }
            else if (tokens[0] == "END")
            {
                instruction[0] = 0x02;
            }
            else if (tokens[0] == "REF")
            {
                instruction[0] = 0x03;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "MOVE")
            {
                instruction[0] = 0x11;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "SWAP")
            {
                instruction[0] = 0x12;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "ROTL")
            {
                instruction[0] = 0x13;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "ROTR")
            {
                instruction[0] = 0x14;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "OR")
            {
                instruction[0] = 0x21;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "NOR")
            {
                instruction[0] = 0x22;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "XOR")
            {
                instruction[0] = 0x23;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "XNOR")
            {
                instruction[0] = 0x24;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "AND")
            {
                instruction[0] = 0x25;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "NAND")
            {
                instruction[0] = 0x26;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "NOT")
            {
                instruction[0] = 0x27;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "ADD")
            {
                instruction[0] = 0x31;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "SUBT")
            {
                instruction[0] = 0x32;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "INCR")
            {
                instruction[0] = 0x33;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "DECR")
            {
                instruction[0] = 0x34;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "MULT")
            {
                instruction[0] = 0x35;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "DIVI")
            {
                instruction[0] = 0x36;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "EXPO")
            {
                instruction[0] = 0x37;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "GOTO")
            {
                instruction[0] = 0x41;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "EVAL")
            {
                instruction[0] = 0x42;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "COMP")
            {
                instruction[0] = 0x43;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                b = ParseArgument(tokens[2]);
                instruction[4] = b[0];
                instruction[5] = b[1];
                instruction[6] = b[2];
            }
            else if (tokens[0] == "JMPZ")
            {
                instruction[0] = 0x44;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "JMGZ")
            {
                instruction[0] = 0x45;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "JMLZ")
            {
                instruction[0] = 0x46;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "GSUB")
            {
                instruction[0] = 0x47;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "RTRN")
            {
                instruction[0] = 0x48;
            }
            else if (tokens[0] == "JMPE")
            {
                instruction[0] = 0x49;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "JMPG")
            {
                instruction[0] = 0x4A;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "JMPL")
            {
                instruction[0] = 0x4B;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "ENQU")
            {
                instruction[0] = 0x51;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            else if (tokens[0] == "DEQU")
            {
                instruction[0] = 0x52;
                byte[] b = ParseArgument(tokens[1]);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
            }
            #endregion
            
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
                throw new NotImplementedException();
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
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
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
