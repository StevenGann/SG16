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
        public Register RAND = (Register)0x00; //RAND will be dealt with differently.
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

        Random RNG = new Random();

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
            Console.Write("Executing " + ASM.ByteArrayToString(instruction.ToArray()));

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
        private void REF(byte[] Arg1, byte[] Arg2)
        {
            RREF = (Register)Arg1;
        }
        private void MOVE(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register to Register
            {
                byte[] data = getRegisterFromID(Arg1[2]);
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal to Register
            {
                byte[] data = new byte[2];
                data[0] = Arg1[2];
                data[1] = Arg1[1];
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute RAM to Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect RAM to Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register to Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal to Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute RAM to Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect RAM to Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register to Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal to Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute RAM to Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect RAM to Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void SWAP(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                setRegisterFromID(Arg2[2], arg1Data);
                setRegisterFromID(Arg1[2], arg2Data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute RAM, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect RAM, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute RAM, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect RAM, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute RAM, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect RAM, Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void ROTL(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00) //Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                arg1Word = (UInt16)(arg1Word << 1);
                byte[] result = UInt16ToByteArray(arg1Word);
                setRegisterFromID(Arg1[2], result);
            }
        }
        private void ROTR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00) //Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                arg1Word = (UInt16)(arg1Word >> 1);
                byte[] result = UInt16ToByteArray(arg1Word);
                setRegisterFromID(Arg1[2], result);
            }
        }
        private void OR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register OR Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                byte[] data = new byte[2];
                data[0] = (byte)((int)arg1Data[0] | (int)arg2Data[0]);
                data[1] = (byte)((int)arg1Data[1] | (int)arg2Data[1]);
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal OR Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute RAM OR Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect RAM OR Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register OR Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal OR Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute RAM OR Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect RAM OR Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register OR Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal OR Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute RAM OR Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect RAM OR Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void NOR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                byte[] data = new byte[2];
                data[0] = (byte)~((int)arg1Data[0] | (int)arg2Data[0]);
                data[1] = (byte)~((int)arg1Data[1] | (int)arg2Data[1]);
                setRegisterFromID(Arg2[2], data);
            }
        }
        private void XOR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register XOR Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                byte[] data = new byte[2];
                data[0] = (byte)((int)arg1Data[0] ^ (int)arg2Data[0]);
                data[1] = (byte)((int)arg1Data[1] ^ (int)arg2Data[1]);
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal XOR Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute XOR AND Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect XOR AND Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register XOR Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal XOR Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute XOR AND Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect XOR AND Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register XOR Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal XOR Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute XOR AND Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect XOR AND Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void XNOR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                byte[] data = new byte[2];
                data[0] = (byte)~((int)arg1Data[0] ^ (int)arg2Data[0]);
                data[1] = (byte)~((int)arg1Data[1] ^ (int)arg2Data[1]);
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute RAM, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect RAM, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute RAM, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect RAM, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute RAM, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect RAM, Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void AND(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register AND Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                byte[] data = new byte[2];
                data[0] = (byte)((int)arg1Data[0] & (int)arg2Data[0]);
                data[1] = (byte)((int)arg1Data[1] & (int)arg2Data[1]);
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal AND Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute RAM AND Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect RAM AND Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register AND Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal AND Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute RAM AND Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect RAM AND Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register AND Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal AND Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute RAM AND Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect RAM AND Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void NAND(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);
                byte[] data = new byte[2];
                data[0] = (byte)~((int)arg1Data[0] & (int)arg2Data[0]);
                data[1] = (byte)~((int)arg1Data[1] & (int)arg2Data[1]);
                setRegisterFromID(Arg2[2], data);
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x00) //Literal, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x00) //Absolute RAM, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x00) //Indirect RAM, Register
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x02) //Register, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x02) //Literal, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x02) //Absolute RAM, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x02) //Indirect RAM, Absolute RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x00 && Arg2[0] == 0x03) //Register, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x01 && Arg2[0] == 0x03) //Literal, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x02 && Arg2[0] == 0x03) //Absolute RAM, Indirect RAM
            {
                throw new NotImplementedException();
            }
            else if (Arg1[0] == 0x03 && Arg2[0] == 0x03) //Indirect RAM, Indirect RAM
            {
                throw new NotImplementedException();
            }
        }
        private void NOT(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00) //Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                arg1Word = (UInt16)~arg1Word;
                byte[] result = UInt16ToByteArray(arg1Word);
                setRegisterFromID(Arg1[2], result);
            }
        }
        private void ADD(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);

                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 arg2Word = (UInt16)(arg2Data[1] << 8 | arg2Data[0]);
                UInt16 resultWord = (UInt16)(arg1Word + arg2Word);

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg2[2], result);
            }
        }
        private void SUBT(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);

                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 arg2Word = (UInt16)(arg2Data[1] << 8 | arg2Data[0]);
                UInt16 resultWord = (UInt16)(arg1Word - arg2Word);

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg2[2], result);
            }
        }
        private void INCR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00) //Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 resultWord = (UInt16)(arg1Word + 1);

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg1[2], result);
            }
        }
        private void DECR(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00) //Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 resultWord = (UInt16)(arg1Word - 1);

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg1[2], result);
            }
        }
        private void MULT(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);

                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 arg2Word = (UInt16)(arg2Data[1] << 8 | arg2Data[0]);
                UInt16 resultWord = (UInt16)(arg1Word * arg2Word);

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg2[2], result);
            }
        }
        private void DIVI(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);

                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 arg2Word = (UInt16)(arg2Data[1] << 8 | arg2Data[0]);
                UInt16 resultWord = (UInt16)(arg2Word / arg1Word);

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg2[2], result);
            }
        }
        private void EXPO(byte[] Arg1, byte[] Arg2)
        {
            if (Arg1[0] == 0x00 && Arg2[0] == 0x00) //Register, Register
            {
                byte[] arg1Data = getRegisterFromID(Arg1[2]);
                byte[] arg2Data = getRegisterFromID(Arg2[2]);

                UInt16 arg1Word = (UInt16)(arg1Data[1] << 8 | arg1Data[0]);
                UInt16 arg2Word = (UInt16)(arg2Data[1] << 8 | arg2Data[0]);
                UInt16 resultWord = (UInt16)(Math.Pow(arg2Word, arg1Word));

                byte[] result = UInt16ToByteArray(resultWord);
                setRegisterFromID(Arg2[2], result);
            }
        }
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

        private void setRegisterFromID(byte id, byte[] value)
        {
            switch(id)
            {
                case 0x00:
                    PC = (Register)value;
                    break;
                case 0x01:
                    STAT = (Register)value;
                    break;
                case 0x02:
                    SUBR = (Register)value;
                    break;
                case 0x03:
                    PSTR = (Register)value;
                    break;
                case 0x04:
                    PEND = (Register)value;
                    break;
                case 0x05:
                    RAND = (Register)value;
                    break;
                case 0x06:
                    RREF = (Register)value;
                    break;
                case 0xF0:
                    USR0 = (Register)value;
                    break;
                case 0xF1:
                    USR1 = (Register)value;
                    break;
                case 0xF2:
                    USR2 = (Register)value;
                    break;
                case 0xF3:
                    USR3 = (Register)value;
                    break;
                case 0xF4:
                    USR4 = (Register)value;
                    break;
                case 0xF5:
                    USR5 = (Register)value;
                    break;
                case 0xF6:
                    USR6 = (Register)value;
                    break;
                case 0xF7:
                    USR7 = (Register)value;
                    break;
                case 0xF8:
                    USR8 = (Register)value;
                    break;
                case 0xF9:
                    USR9 = (Register)value;
                    break;
                case 0xFA:
                    USRA = (Register)value;
                    break;
                case 0xFB:
                    USRB = (Register)value;
                    break;
                case 0xFC:
                    USRC = (Register)value;
                    break;
                case 0xFD:
                    USRD = (Register)value;
                    break;
                case 0xFE:
                    USRE = (Register)value;
                    break;
                case 0xFF:
                    USRF = (Register)value;
                    break;
                default:
                    break;
            }
        }

        private byte[] getRegisterFromID(byte id)
        {
            byte[] result = new byte[2];

            switch (id)
            {
                case 0x00:
                    result[0] = PC.LowerByte;
                    result[1] = PC.UpperByte;
                    break;
                case 0x01:
                    result[0] = STAT.LowerByte;
                    result[1] = STAT.UpperByte;
                    break;
                case 0x02:
                    result[0] = SUBR.LowerByte;
                    result[1] = SUBR.UpperByte;
                    break;
                case 0x03:
                    result[0] = PSTR.LowerByte;
                    result[1] = PSTR.UpperByte;
                    break;
                case 0x04:
                    result[0] = PEND.LowerByte;
                    result[1] = PEND.UpperByte;
                    break;
                case 0x05:
                    RNG.NextBytes(result);
                    break;
                case 0x06:
                    result[0] = RREF.LowerByte;
                    result[1] = RREF.UpperByte;
                    break;
                case 0xF0:
                    result[0] = USR0.LowerByte;
                    result[1] = USR0.UpperByte;
                    break;
                case 0xF1:
                    result[0] = USR1.LowerByte;
                    result[1] = USR1.UpperByte;
                    break;
                case 0xF2:
                    result[0] = USR2.LowerByte;
                    result[1] = USR2.UpperByte;
                    break;
                case 0xF3:
                    result[0] = USR3.LowerByte;
                    result[1] = USR3.UpperByte;
                    break;
                case 0xF4:
                    result[0] = USR4.LowerByte;
                    result[1] = USR4.UpperByte;
                    break;
                case 0xF5:
                    result[0] = USR5.LowerByte;
                    result[1] = USR5.UpperByte;
                    break;
                case 0xF6:
                    result[0] = USR6.LowerByte;
                    result[1] = USR6.UpperByte;
                    break;
                case 0xF7:
                    result[0] = USR7.LowerByte;
                    result[1] = USR7.UpperByte;
                    break;
                case 0xF8:
                    result[0] = USR8.LowerByte;
                    result[1] = USR8.UpperByte;
                    break;
                case 0xF9:
                    result[0] = USR9.LowerByte;
                    result[1] = USR9.UpperByte;
                    break;
                case 0xFA:
                    result[0] = USRA.LowerByte;
                    result[1] = USRA.UpperByte;
                    break;
                case 0xFB:
                    result[0] = USRB.LowerByte;
                    result[1] = USRB.UpperByte;
                    break;
                case 0xFC:
                    result[0] = USRC.LowerByte;
                    result[1] = USRC.UpperByte;
                    break;
                case 0xFD:
                    result[0] = USRD.LowerByte;
                    result[1] = USRD.UpperByte;
                    break;
                case 0xFE:
                    result[0] = USRE.LowerByte;
                    result[1] = USRE.UpperByte;
                    break;
                case 0xFF:
                    result[0] = USRF.LowerByte;
                    result[1] = USRF.UpperByte;
                    break;
                default:
                    break;
            }

            return result;
        }

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

        static byte[] UInt16ToByteArray(UInt16 b)
        {
            byte[] result = new byte[2];

            if (b <= 0xFF)
            {
                result[0] = Convert.ToByte(b);
            }
            else
            {
                result[0] = 0xFF;
                result[1] = Convert.ToByte(b - Convert.ToUInt16(0xFF));
            }

            return result;
        }
    }
}
