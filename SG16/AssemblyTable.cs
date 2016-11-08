using System;
using System.Collections.Generic;

namespace SG16
{
    public class AssemblyTable
    {
        //ALL Assembly instruction and machine code translations will stay
        //in this class. This makes the Assembler and Disassembler much
        //easier to maintain.
        private List<string> Instructions = new List<string>();

        private List<byte> Opcodes = new List<byte>();
        private List<string> Registers = new List<string>();
        private List<byte> IDs = new List<byte>();

        public AssemblyTable()
        {
            // Instructions
            AddInstruction("NULL", 0x00);
            AddInstruction("START", 0x01);
            AddInstruction("END", 0x02);
            AddInstruction("REF", 0x03);
            //-----------------
            AddInstruction("MOVE", 0x11);
            AddInstruction("SWAP", 0x12);
            AddInstruction("ROTL", 0x13);
            AddInstruction("ROTR", 0x14);
            //-----------------
            AddInstruction("OR", 0x21);
            AddInstruction("NOR", 0x22);
            AddInstruction("XOR", 0x23);
            AddInstruction("XNOR", 0x24);
            AddInstruction("AND", 0x25);
            AddInstruction("NAND", 0x26);
            AddInstruction("NOT", 0x27);
            //-----------------
            AddInstruction("ADD", 0x31);
            AddInstruction("SUBT", 0x32);
            AddInstruction("INCR", 0x33);
            AddInstruction("DECR", 0x34);
            AddInstruction("MULT", 0x35);
            AddInstruction("DIVI", 0x36);
            AddInstruction("EXPO", 0x37);
            //-----------------
            AddInstruction("GOTO", 0x41);
            AddInstruction("EVAL", 0x42);
            AddInstruction("COMP", 0x43);
            AddInstruction("JMPZ", 0x44);
            AddInstruction("JMGZ", 0x45);
            AddInstruction("JMLZ", 0x46);
            AddInstruction("GSUB", 0x47);
            AddInstruction("RTRN", 0x48);
            AddInstruction("JMPE", 0x49);
            AddInstruction("JMPG", 0x4A);
            AddInstruction("JMPL", 0x4B);
            //-----------------
            AddInstruction("ENQU", 0x51);
            AddInstruction("DEQU", 0x52);
            AddInstruction("PUSH", 0x53);
            AddInstruction("POP", 0x54);
            //-----------------
            AddInstruction("TXD0", 0x61);
            AddInstruction("RXD0", 0x62);
            AddInstruction("TXD1", 0x63);
            AddInstruction("RXD1", 0x64);
            AddInstruction("ROMR", 0x65);
            AddInstruction("ROMW", 0x66);
            //-----------------
            // Registers
            AddRegister("PC", 0x00);
            AddRegister("STAT", 0x01);
            AddRegister("SUBR", 0x02);
            AddRegister("PSTR", 0x03);
            AddRegister("PEND", 0x04);
            AddRegister("RAND", 0x05);
            AddRegister("RREF", 0x06);
            AddRegister("PAGE", 0x07);
            AddRegister("MEMS", 0x08);
            AddRegister("PEEK", 0x09);
            //-----------------
            AddRegister("UART0", 0xA0);
            AddRegister("UART1", 0xA1);
            //-----------------
            AddRegister("BUS0", 0xB0);
            AddRegister("BDAT", 0xB1);
            //-----------------
            AddRegister("USR0", 0xF0);
            AddRegister("USR1", 0xF1);
            AddRegister("USR2", 0xF2);
            AddRegister("USR3", 0xF3);
            AddRegister("USR4", 0xF4);
            AddRegister("USR5", 0xF5);
            AddRegister("USR6", 0xF6);
            AddRegister("USR7", 0xF7);
            AddRegister("USR8", 0xF8);
            AddRegister("USR9", 0xF9);
            AddRegister("USRA", 0xFA);
            AddRegister("USRB", 0xFB);
            AddRegister("USRC", 0xFC);
            AddRegister("USRD", 0xFD);
            AddRegister("USRE", 0xFE);
            AddRegister("USRF", 0xFF);
        }

        public bool ContainsInstruction(string _instruction)
        {
            return Instructions.Contains(_instruction);
        }

        public bool ContainsOpcode(byte _opcode)
        {
            return Opcodes.Contains(_opcode);
        }

        public bool ContainsRegister(string _register)
        {
            return Registers.Contains(_register);
        }

        public bool ContainsID(byte _id)
        {
            return IDs.Contains(_id);
        }

        public byte GetOpcode(string instruction)
        {
            for (int i = 0; i < Instructions.Count; i++)
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
            for (int i = 0; i < Opcodes.Count; i++)
            {
                if (Opcodes[i] == opcode)
                {
                    return Instructions[i];
                }
            }
            throw new Exception("Opcode not recognized");
        }

        public byte GetID(string register)
        {
            for (int i = 0; i < Registers.Count; i++)
            {
                if (Registers[i] == register.ToUpper())
                {
                    return IDs[i];
                }
            }
            throw new Exception("Register not recognized");
        }

        public string GetRegister(byte id)
        {
            for (int i = 0; i < IDs.Count; i++)
            {
                if (IDs[i] == id)
                {
                    return Registers[i];
                }
            }
            throw new Exception("Register ID not recognized");
        }

        private void AddInstruction(string i, byte o)
        {
            Instructions.Add(i);
            Opcodes.Add(o);
        }

        private void AddRegister(string r, byte i)
        {
            Registers.Add(r);
            IDs.Add(i);
        }
    }
}