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
                }
                else if (instructions[i][0] == '#') //Check for comment
                {
                    instructions.RemoveAt(i);
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

            //TODO: Actually assemble machine code from tokenized Assembly code
            
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
