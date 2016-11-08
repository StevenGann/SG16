using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SG16
{
    public class ASM
    {
        public void Assemble(string _input, string _output)
        {
            bool _debug = false;

            //Split code file into lines
            string[] inputArray = _input.Split('\n');
            List<string> instructions = new List<string>(inputArray);
            Dictionary<string, int> labels = new Dictionary<string, int>();

            if (_debug)
            {
                Console.Write("\nInput file before parsing:\n");
                for (int i = 0; i < instructions.Count; i++)
                {
                    Console.WriteLine(i + "\t|" + instructions[i]);
                }
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
            }

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
                else if (instructions[i][0] == '#') //Check for whole-line comment
                {
                    instructions.RemoveAt(i);
                    i--;
                }
                else
                {
                    //Check for inline comment
                    for (int j = 0; j < instructions[i].Length; j++)
                    {
                        if (instructions[i][j] == '#')
                        {
                            instructions[i] = instructions[i].Remove(j, instructions[i].Length - j);
                        }
                    }
                    instructions[i] = instructions[i].Trim();
                }
            }

            if (_debug)
            {
                Console.Write("\nComments and whitespace removed:\n");
                for (int i = 0; i < instructions.Count; i++)
                {
                    Console.WriteLine(i + "\t|" + instructions[i]);
                }
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
            }

            //Index and remove labels, and remove anything that is neither
            for (int i = 0; i < instructions.Count; i++)
            {
                if (!isInstruction(instructions[i]))
                {
                    if (isLabel(instructions[i]))
                    {
                        string label = tokenizeLine(instructions[i])[0];
                        label = label.Remove(label.Length - 1);
                        labels.Add(label, i);
                        Console.WriteLine("Found label \"" + label + "\" at @" + ByteToString(Convert.ToByte(i * 8)) + "\t Line " + Convert.ToString(i));
                    }

                    instructions.RemoveAt(i);
                    i--;
                }
            }

            if (_debug)
            {
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Write("\nLabels removed:\n");
                for (int i = 0; i < instructions.Count; i++)
                {
                    Console.WriteLine(i + "\t|" + instructions[i]);
                }
                Console.WriteLine("Press ENTER to assemble");
                Console.ReadLine();
            }

            //Convert each Assembly instruction to machine code
            List<byte> program = new List<byte>();
            foreach (string line in instructions)
            {
                Console.WriteLine("--------------");
                Console.WriteLine(line);
                byte[] code = StringToMachineCode(line, labels);
                Console.WriteLine(ByteArrayToString(code));
                program.AddRange(code);
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

        private bool isInstruction(string _line)
        {
            bool result = false;

            List<string> tokens = tokenizeLine(_line);

            if (tokens.Count > 0)
            {
                AssemblyTable table = new AssemblyTable();
                if (table.ContainsInstruction(tokens[0]))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool isLabel(string _line)
        {
            //The specification requires label definitions to be
            // - All uppercase letters or _
            // - The only thing on the line
            // - Terminated with a :
            //
            //This currently does not enforce the first restriction, and will let numbers
            //and other symbols in. Perhaps the specification SHOULD allow numbers in
            //labels, as well as printable symbols like |, $, etc.
            bool result = false;

            List<string> tokens = tokenizeLine(_line);

            if (tokens.Count == 1)                                  //Labels must be the only thing on that line
            {
                if (tokens[0][tokens[0].Length - 1] == ':')         //Labels must end with a :
                {
                    if (tokens[0] == tokens[0].ToUpper())           //Labels must be all uppercase
                    {
                        //This check goes last because it is the most resource intensive.
                        //See? I do optimize sometimes!
                        AssemblyTable table = new AssemblyTable();
                        if (!table.ContainsInstruction(tokens[0]))  //Labels cannot be an instruction
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private List<string> tokenizeLine(string _input)
        {
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

            return tokens;
        }

        private byte[] StringToMachineCode(string _input, Dictionary<string, int> _labels)
        {
            bool _debug = false;
            byte[] instruction = new byte[8];

            instruction[0] = 0x00; //NULL opcode
            instruction[1] = 0x01; //Reference type Literal
            instruction[2] = 0x00; //Data upper byte
            instruction[3] = 0x00; //Data lower byte
            instruction[4] = 0x01; //Reference type Literal
            instruction[5] = 0x00; //Data upper byte
            instruction[6] = 0x00; //Data lower byte
            instruction[7] = 0xFF; //RESERVED byte

            List<string> tokens = tokenizeLine(_input);

            if (_debug)
            {
                Console.WriteLine("Raw:\n" + _input);
                Console.WriteLine("Tokenized:");
                foreach (string t in tokens) { Console.Write(" | " + t); }
                Console.WriteLine(" |");
                Console.ReadLine();
            }

            AssemblyTable table = new AssemblyTable();
            instruction[0] = table.GetOpcode(tokens[0]);
            if (tokens.Count >= 2)
            {
                byte[] b = ParseArgument(tokens[1], _labels);
                instruction[1] = b[0];
                instruction[2] = b[1];
                instruction[3] = b[2];
                if (tokens.Count >= 3)
                {
                    b = ParseArgument(tokens[2], _labels);
                    instruction[4] = b[0];
                    instruction[5] = b[1];
                    instruction[6] = b[2];

                    if (tokens.Count > 3)
                    {
                        throw new Exception("Too many parameters");
                    }
                }
            }
            return instruction;
        }

        private bool isRegister(string _token)
        {
            bool result = false;

            if (_token == _token.ToUpper() && _token.Length >= 2 && _token.Length <= 4)//If it looks like a register
            {
                AssemblyTable table = new AssemblyTable();

                result = table.ContainsRegister(_token);
            }

            return result;
        }

        public byte[] ParseArgument(string _input, Dictionary<string, int> _labels)
        {
            string input = _input;
            //Console.WriteLine("Argument: " + input);
            byte[] result = new byte[3];
            result[0] = 0x01;//Failsafe value, 0x0000 literal.
            result[1] = 0x00;
            result[2] = 0x00;

            int byteMode = 0;

            //If a suffix is attached, identify and remove it.
            //NOTE: s.U is valid syntax for a 16-bit ASCII literal. Check for it!
            if (input[input.Length - 2] == '.' && input[0] != 's')
            {
                if (input[input.Length - 1] == 'L')
                {
                    byteMode = 1;
                }
                else if (input[input.Length - 1] == 'U')
                {
                    byteMode = 2;
                }
                else
                {
                    throw new Exception("\nSyntax error in \"" + input + "\"");
                }
                input = input.Remove(input.Length - 2, 2);
            }

            if (input[0] == 's')//ASCII literal
            {
                result[0] = 0x01;
                string raw = input.Remove(0, 1);
                UInt16 upper = 0;
                UInt16 lower = 0;
                try
                {
                    if (raw.Length == 2)//16-bit
                    {
                        upper = Convert.ToUInt16(raw[0]);
                        lower = Convert.ToUInt16(raw[1]);
                    }
                    else if (raw.Length == 1)//8-bit
                    {
                        upper = 0;
                        lower = Convert.ToUInt16(raw[0]);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    throw new Exception("\nSyntax Error in \"" + input + "\"");
                }

                result[1] = BitConverter.GetBytes(upper)[0];
                result[2] = BitConverter.GetBytes(lower)[0];
            }
            else if (input[0] == 'o')//Octal literal
            {
                result[0] = 0x01;
                throw new NotImplementedException();
            }
            else if (input[0] == 'x')//Hex literal
            {
                result[0] = 0x01;//Mark it as a literal type
                string raw = input.Remove(0, 1);//Remove the type symbol
                if (raw.Length == 2)//8-bit literal
                {
                    byte[] data = StringToByteArray(raw);
                    result[1] = 0x00;
                    result[2] = data[0];
                }
                else if (raw.Length == 4)//16-bit literal
                {
                    byte[] data = StringToByteArray(raw);
                    result[1] = data[0];
                    result[2] = data[1];
                }
                else
                {
                    throw new Exception("\nSyntax error in \"" + input + "\"");
                }
            }
            else if (input[0] == 'd')//Decimal literal
            {
                result[0] = 0x01;
                UInt16 raw = 0;
                try
                {
                    raw = Convert.ToUInt16(int.Parse(input.Remove(0, 1)));
                }
                catch
                {
                    throw new Exception("\nSyntax Error in \"" + input + "\"");
                }

                byte[] data = BitConverter.GetBytes(raw);
                result[1] = data[0];
                result[2] = data[1];
            }
            else if (input[0] == 'b')//Binary literal
            {
                result[0] = 0x01;
                throw new NotImplementedException();
            }
            else if (input[0] == '@')//Absolute address
            {
                //There's no support for an 8-bit literal address.
                //Frankly, I'm not sure how it'd be useful.
                string raw = input.Remove(0, 1);
                if (!isRegister(raw))
                {
                    //Translate literal hex value to address
                    result[0] = 0x02;
                    byte[] data = StringToByteArray(raw);
                    result[1] = data[0];
                    result[2] = data[1];
                }
                else
                {
                    //It's a register. Dereference it.
                    result[0] = 0x04;
                    result[1] = 0x00;
                    result[2] = new AssemblyTable().GetID(raw); //Wew that's terse.
                }
            }
            else if (input[0] == '$')//Indirect address
            {
                //There's no support for an 8-bit literal offset.
                //Frankly, I'm not sure how it'd be useful.
                string raw = input.Remove(0, 1);
                if (!isRegister(raw))
                {
                    //Translate literal hex value to offset
                    result[0] = 0x03;
                    byte[] data = StringToByteArray(raw);
                    result[1] = data[0];
                    result[2] = data[1];
                }
                else
                {
                    //It's a register. Dereference it.
                    result[0] = 0x05;
                    result[1] = 0x00;
                    result[2] = new AssemblyTable().GetID(raw); //Wew that's terse.
                }
            }
            else if (_labels.ContainsKey(input))//Label, convert it to an Absolute address
            {
                //Console.WriteLine("\nParsing label");
                result[0] = 0x02;
                string raw = input;
                //Console.WriteLine("Label: " + raw);
                byte[] data = BitConverter.GetBytes(_labels[raw]);
                result[1] = data[0];
                result[2] = data[1];
                //Console.WriteLine("Address: " + ByteArrayToString(data));
            }
            else //Must be a register
            {
                result[0] = 0x00;
                result[1] = 0x00;
                result[2] = new AssemblyTable().GetID(input);
            }

            //Apply upper/lower byte mode
            if (byteMode != 0)
            {
                if (byteMode == 1)//Lower byte
                {
                    result[0] += 0x10;
                }
                else if (byteMode == 2)//Upper byte
                {
                    result[0] += 0x20;
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

        public static string ByteToString(byte b)
        {
            StringBuilder hex = new StringBuilder(2);
            hex.AppendFormat("{0:x2}", b);
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