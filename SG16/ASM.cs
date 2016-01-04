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

            string[] tokens = _input.Split(' ');

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
            }
            else if (tokens[0] == "MOVE")
            {
                instruction[0] = 0x11;
            }
            else if (tokens[0] == "SWAP")
            {
                instruction[0] = 0x12;
            }
            else if (tokens[0] == "ROTL")
            {
                instruction[0] = 0x13;
            }
            else if (tokens[0] == "ROTR")
            {
                instruction[0] = 0x14;
            }
            else if (tokens[0] == "OR")
            {
                instruction[0] = 0x21;
            }
            else if (tokens[0] == "NOR")
            {
                instruction[0] = 0x22;
            }
            else if (tokens[0] == "XOR")
            {
                instruction[0] = 0x23;
            }
            else if (tokens[0] == "XNOR")
            {
                instruction[0] = 0x24;
            }
            else if (tokens[0] == "AND")
            {
                instruction[0] = 0x25;
            }
            else if (tokens[0] == "NAND")
            {
                instruction[0] = 0x26;
            }
            else if (tokens[0] == "NOT")
            {
                instruction[0] = 0x27;
            }
            else if (tokens[0] == "ADD")
            {
                instruction[0] = 0x31;
            }
            else if (tokens[0] == "SUBT")
            {
                instruction[0] = 0x32;
            }
            else if (tokens[0] == "INCR")
            {
                instruction[0] = 0x33;
            }
            else if (tokens[0] == "DECR")
            {
                instruction[0] = 0x34;
            }
            else if (tokens[0] == "MULT")
            {
                instruction[0] = 0x35;
            }
            else if (tokens[0] == "DIVI")
            {
                instruction[0] = 0x36;
            }
            else if (tokens[0] == "EXPO")
            {
                instruction[0] = 0x37;
            }
            else if (tokens[0] == "GOTO")
            {
                instruction[0] = 0x41;
            }
            else if (tokens[0] == "EVAL")
            {
                instruction[0] = 0x42;
            }
            else if (tokens[0] == "COMP")
            {
                instruction[0] = 0x43;
            }
            else if (tokens[0] == "JMPZ")
            {
                instruction[0] = 0x44;
            }
            else if (tokens[0] == "JMGZ")
            {
                instruction[0] = 0x45;
            }
            else if (tokens[0] == "JMLZ")
            {
                instruction[0] = 0x46;
            }
            else if (tokens[0] == "GSUB")
            {
                instruction[0] = 0x47;
            }
            else if (tokens[0] == "RTRN")
            {
                instruction[0] = 0x48;
            }
            else if (tokens[0] == "JMPE")
            {
                instruction[0] = 0x49;
            }
            else if (tokens[0] == "JMPG")
            {
                instruction[0] = 0x4A;
            }
            else if (tokens[0] == "JMPL")
            {
                instruction[0] = 0x4B;
            }
            else if (tokens[0] == "ENQU")
            {
                instruction[0] = 0x51;
            }
            else if (tokens[0] == "DEQU")
            {
                instruction[0] = 0x52;
            }


            return instruction;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
