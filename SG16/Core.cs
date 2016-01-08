using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SG16
{
    public class Core
    {
        //System Registers
        public Register PC = (Register)0x00;
        public Register STAT = (Register)0x00;
        public Register SUBR = (Register)0x00;
        public Register PSTR = (Register)0x00;
        public Register PEND = (Register)0x00;
        //public Register RAND = (Register)0x00; //RAND will be dealt with differently.
        public Register RREF = (Register)0x00;

        //User Registers
        public Register USR0 = (Register)0x00;
        public Register USR1 = (Register)0x00;
        public Register USR2 = (Register)0x00;
        public Register USR3 = (Register)0x00;
        public Register USR4 = (Register)0x00;
        public Register USR5 = (Register)0x00;
        public Register USR6 = (Register)0x00;
        public Register USR7 = (Register)0x00;
        public Register USR8 = (Register)0x00;
        public Register USR9 = (Register)0x00;
        public Register USRA = (Register)0x00;
        public Register USRB = (Register)0x00;
        public Register USRC = (Register)0x00;
        public Register USRD = (Register)0x00;
        public Register USRE = (Register)0x00;
        public Register USRF = (Register)0x00;

        public Memory RAM = new Memory();

        public long Tick()
        {
            Stopwatch sw = Stopwatch.StartNew();

            //Execute Instruction
            Execute(PC.ToInt());
            
            //Advance PC
            PC.Increment(8);

            long result = sw.ElapsedMilliseconds;
            sw.Stop();
            return result;
        }

        private void Execute(int address)
        {
            Instruction instruction = new Instruction(RAM.Data, address);
            Console.WriteLine("Executing " + ASM.ByteArrayToString(instruction.ToArray()));

            switch (instruction.Opcode)
            {
                case 0x00:
                    NULL(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x01:
                    START(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x02:
                    END(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x03:
                    REF(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x11:
                    MOVE(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x12:
                    SWAP(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x13:
                    ROTL(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x14:
                    ROTR(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x21:
                    OR(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x22:
                    NOR(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x23:
                    XOR(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x24:
                    XNOR(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x25:
                    AND(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x26:
                    NAND(instruction.Argument1, instruction.Argument2);
                    break;
                case 0x27:
                    NOT(instruction.Argument1, instruction.Argument2);
                    break;
                default:
                    NULL(instruction.Argument1, instruction.Argument2);
                    break;
            }

        }

        #region Operations
        private void NULL(byte[] Arg1, byte[] Arg2) { } //Does nothing
        private void START(byte[] Arg1, byte[] Arg2)
        {
            PSTR = (Register)PC.ToInt();
        }
        private void END(byte[] Arg1, byte[] Arg2)
        {
            PEND = (Register)PC.ToInt();
            PC = (Register)PSTR.ToInt();
        }
        private void REF(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void MOVE(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void SWAP(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void ROTL(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void ROTR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void OR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void NOR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void XOR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void XNOR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void AND(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void NAND(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void NOT(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void ADD(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void SUBT(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void INCR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void DECR(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void MULT(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void DIVI(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void EXPO(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void GOTO(byte[] Arg1, byte[] Arg2)
        {
            PC = (Register)Arg1;
        }
        private void EVAL(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void COMP(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void JMPZ(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void JMGZ(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void JMLZ(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void GSUB(byte[] Arg1, byte[] Arg2)
        {
            SUBR = (Register)PC.ToInt();
            PC = (Register)Arg1;
        }
        private void RTRN(byte[] Arg1, byte[] Arg2)
        {
            PC = (Register)(SUBR.ToInt() + 1);
        }
        private void JMPE(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void JMPG(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void JMPL(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void ENQU(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }
        private void DEQU(byte[] Arg1, byte[] Arg2) { throw new NotImplementedException(); }

        #endregion



        public void LoadROM(int offset, string path)
        {
            if (File.Exists(path))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    int length = (int)reader.BaseStream.Length;
                    byte[] chunk = new byte[8];
                    int n = 0;
                    for (int i = 0; i < length; i++)
                    {
                        RAM[i + offset] = reader.ReadByte();
                        chunk[n] = RAM[i + offset];
                        n++;
                        if (n >= 8)
                        {
                            n = 0;
                            Console.WriteLine("Loaded " + ASM.ByteArrayToString(chunk));
                        }

                    }
                }
            }
        }

        public void LoadROM(string path)
        {
            LoadROM(0, path);
        }
    }
}
