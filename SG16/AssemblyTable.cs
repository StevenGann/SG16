using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class AssemblyTable
    {
        //ALL Assembly instruction and machine code translations will stay
        //in this class. This makes the Assembler and Disassembler much
        //easier to maintain.
        private List<string> Instructions = new List<string>();
        private List<byte> Opcodes = new List<byte>();

        public AssemblyTable()
        {
            Add("NULL", 0x00);
            Add("START", 0x01);
            Add("END", 0x02);
            Add("REF", 0x03);
            Add("MOVE", 0x11);
            Add("SWAP", 0x12);
            Add("ROTL", 0x13);
            Add("ROTR", 0x14);
            Add("OR", 0x21);
            Add("NOR", 0x22);
            Add("XOR", 0x23);
            Add("XNOR", 0x24);
            Add("AND", 0x25);
            Add("NAND", 0x26);
            Add("NOT", 0x27);
            Add("ADD", 0x31);
            Add("SUBT", 0x32);
            Add("INCR", 0x33);
            Add("DECR", 0x34);
            Add("MULT", 0x35);
            Add("DIVI", 0x36);
            Add("EXPO", 0x37);
            Add("GOTO", 0x41);
            Add("EVAL", 0x42);
            Add("COMP", 0x43);
            Add("JMPZ", 0x44);
            Add("JMGZ", 0x45);
            Add("JMLZ", 0x46);
            Add("GSUB", 0x47);
            Add("RTRN", 0x48);
            Add("JMPE", 0x49);
            Add("JMPG", 0x4A);
            Add("JMPL", 0x4B);
            Add("ENQU", 0x51);
            Add("DEQU", 0x52);
        }

        public byte GetOpcode(string instruction)
        {
            for (int i = 0; i <= Instructions.Count; i++)
            {
                if (Instructions[i] == instruction.ToUpper())
                {
                    return Opcodes[i];
                }
            }
            throw new Exception("Instruction not recognized");
        }

        public string GetInstruction(byte opcode)
        {
            for (int i = 0; i <= Opcodes.Count; i++)
            {
                if (Opcodes[i] == opcode)
                {
                    return Instructions[i];
                }
            }
            throw new Exception("Opcode not recognized");
        }

        private void Add(string i, byte o)
        {
            Instructions.Add(i);
            Opcodes.Add(o);
        }
    }
}
