using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SG16
{
    public class Core
    {
        //System Registers
        public Register PC = (Register)0x00;

        public StatusRegister STAT = (StatusRegister)0x00;
        public Register SUBR = (Register)0x00;
        public Register PSTR = (Register)0x00;
        public Register PEND = (Register)0x00;
        public RandomRegister RAND = new RandomRegister();
        public Register RREF = (Register)0x00;
        public Register PAGE = (Register)0x00;
        public Register MEMS = (Register)0x00;
        public Register PEEK = (Register)0x00;

        //Peripheral Configuration
        public Register UART0 = (Register)0x00;

        public Register UART1 = (Register)0x00;

        //Peripheral Bus
        public Register BUS0 = (Register)0x00;

        public Register BDAT = (Register)0x00;

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
        public Memory ROM = new Memory();

        public List<Byte> TXD0buffer = new List<byte>();
        public List<Byte> TXD1buffer = new List<byte>();
        public List<Byte> RXD0buffer = new List<byte>();
        public List<Byte> RXD1buffer = new List<byte>();

        private int lastPAGE = 0;

        private AssemblyTable Table = new AssemblyTable();

        public string Message = "";
        public string CachePath = Directory.GetCurrentDirectory() + "/cache/";
        public bool Debug = true;
        public bool Fast = true;
        public bool Verbose = false;
        private Stopwatch sw = new Stopwatch();

        static Core()
        {
        }

        public long Tick()
        {
            if (Debug) { sw = Stopwatch.StartNew(); }

            lastPAGE = PAGE.ToInt();

            //Execute Instruction
            Execute(PC.ToInt());

            //Advance PC
            PC.Increment(8);

            if (PAGE.ToInt() != lastPAGE)
            {
                //Swap RAM pages
                string oldRAMFile = CachePath + "RAM_Page." + lastPAGE.ToString("00000") + ".dat";
                using (BinaryWriter writer = new BinaryWriter(File.Open(oldRAMFile, FileMode.Create)))
                {
                    for (int i = 0; i < RAM.Size; i++)
                    {
                        writer.Write(RAM[i]);
                    }
                }

                RAM = new Memory();
                string newRAMFile = CachePath + "RAM_Page." + PAGE.ToInt().ToString("00000") + ".dat";
                if (File.Exists(newRAMFile))
                {
                    //Load file
                    using (BinaryReader reader = new BinaryReader(File.Open(newRAMFile, FileMode.Open)))
                    {
                        int length = (int)reader.BaseStream.Length;
                        byte[] chunk = new byte[8];
                        int n = 0;
                        for (int i = 0; i < length; i++)
                        {
                            ROM[i] = reader.ReadByte();
                            chunk[n] = ROM[i];
                            n++;
                            if (n >= 8 && Debug)
                            {
                                n = 0;
                                Message = "Loaded " + ASM.ByteArrayToString(chunk);
                            }
                        }
                    }
                }
            }

            if (Debug)
            {
                long result = sw.ElapsedMilliseconds;
                sw.Stop();
                return result;
            }
            else
            {
                return 0;
            }
        }

        public void Initialize()
        {
            int i = 0;
            int j = 0;
            bool ended = false;

            byte[] chunk = new byte[8];

            while (!ended)
            {
                RAM[i] = ROM[i];
                chunk[j] = ROM[i];
                i++;
                j++;

                if (j >= 8)
                {
                    j = 0;
                    if (Table.GetInstruction(chunk[0]) == "END")
                    {
                        ended = true;
                    }
                }

                if (i >= ROM.Size) { throw new Exception("Unterminated Bootloader"); }
            }
        }

        private void Execute(int address)
        {
            Instruction instruction = new Instruction(RAM.Data, address);
            if (Debug) { Message = "Executing " + ASM.ByteArrayToString(instruction.ToArray()); }

            switch (instruction.Opcode)
            {
                //System Instructions
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

                //Memory Operations
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

                //Logic Operations
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

                //Math Operations
                case 0x31:
                    ADD(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x32:
                    SUBT(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x33:
                    INCR(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x34:
                    DECR(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x35:
                    MULT(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x36:
                    DIVI(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x37:
                    EXPO(instruction.Argument1, instruction.Argument2);
                    break;

                //Flow Control
                case 0x41:
                    GOTO(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x42:
                    EVAL(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x43:
                    COMP(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x44:
                    JMPZ(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x45:
                    JMGZ(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x46:
                    JMLZ(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x47:
                    GSUB(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x48:
                    RTRN(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x49:
                    JMPE(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x4A:
                    JMPG(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x4B:
                    JMPL(instruction.Argument1, instruction.Argument2);
                    break;

                //Special Operations
                case 0x51:
                    ENQU(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x52:
                    DEQU(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x53:
                    PUSH(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x54:
                    POP(instruction.Argument1, instruction.Argument2);
                    break;

                //Peripherals
                case 0x61:
                    TXD0(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x62:
                    RXD0(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x63:
                    TXD1(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x64:
                    RXD1(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x65:
                    ROMR(instruction.Argument1, instruction.Argument2);
                    break;

                case 0x66:
                    ROMW(instruction.Argument1, instruction.Argument2);
                    break;

                default:
                    NULL(instruction.Argument1, instruction.Argument2);
                    break;
            }
        }

        #region Operations

        private void NULL(byte[] Arg1, byte[] Arg2)
        {
        } //Does nothing

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
            //==================================
            //REF Arg1
            //----------------------------------
            //Supports all valid data types
            //Copies value from Arg1 into RREF
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            RREF = (Register)data;
        }

        private void MOVE(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //MOVE Arg1 Arg2
            //----------------------------------
            //Supports all valid data types
            //Copies value from Arg1 into location Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            setDataFromParameter(Arg2, data1);
        }

        private void SWAP(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //SWAP Arg1 Arg2
            //----------------------------------
            //Supports all data types, EXCEPT literals
            //Swaps values between locations Arg1 and Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                //Arg1Type == 0x01 ||//Cannot swap with a literal
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot swap with a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            setDataFromParameter(Arg1, data2);
            setDataFromParameter(Arg2, data1);
        }

        private void ROTL(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ROTL Arg1
            //----------------------------------
            //Supports all data types EXCEPT literals
            //Binary rotates left
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||//
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)(data[1] << 1);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)(data[0] << 1);
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data[1] << 8 | data[0]);
                arg1Word = (UInt16)(arg1Word << 1);
                data = UInt16ToByteArray(arg1Word);
            }

            setDataFromParameter(Arg1, data);
        }

        private void ROTR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ROTR Arg1
            //----------------------------------
            //Supports all data types EXCEPT literals
            //Binary rotates right
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||//
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)(data[1] >> 1);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)(data[0] >> 1);
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data[1] << 8 | data[0]);
                arg1Word = (UInt16)(arg1Word >> 1);
                data = UInt16ToByteArray(arg1Word);
            }

            setDataFromParameter(Arg1, data);
        }

        private void OR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //OR Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Logic OR of Arg1 and Arg2 is stored at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)((int)data1[1] | (int)data2[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)((int)data1[0] | (int)data2[0]);
                }
            }
            else
            {
                data[0] = (byte)((int)data1[0] | (int)data2[0]);
                data[1] = (byte)((int)data1[1] | (int)data2[1]);
            }

            setDataFromParameter(Arg2, data1);
        }

        private void NOR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //NOR Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Logical NOR of Arg1 and Arg2 is stored at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)~((int)data1[1] | (int)data2[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)~((int)data1[0] | (int)data2[0]);
                }
            }
            else
            {
                data[0] = (byte)~((int)data1[0] | (int)data2[0]);
                data[1] = (byte)~((int)data1[1] | (int)data2[1]);
            }

            setDataFromParameter(Arg2, data1);
        }

        private void XOR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //XOR Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Logical XOR of Arg1 and Arg2 is stored at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)((int)data1[1] ^ (int)data2[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)((int)data1[0] ^ (int)data2[0]);
                }
            }
            else
            {
                data[0] = (byte)((int)data1[0] ^ (int)data2[0]);
                data[1] = (byte)((int)data1[1] ^ (int)data2[1]);
            }

            setDataFromParameter(Arg2, data1);
        }

        private void XNOR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //XNOR Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Logical XNOR of Arg1 and Arg2 is stored at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)~((int)data1[1] ^ (int)data2[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)~((int)data1[0] ^ (int)data2[0]);
                }
            }
            else
            {
                data[0] = (byte)~((int)data1[0] ^ (int)data2[0]);
                data[1] = (byte)~((int)data1[1] ^ (int)data2[1]);
            }

            setDataFromParameter(Arg2, data1);
        }

        private void AND(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //AND Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Logical AND of Arg1 and Arg2 is stored at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)((int)data1[1] & (int)data2[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)((int)data1[0] & (int)data2[0]);
                }
            }
            else
            {
                data[0] = (byte)((int)data1[0] & (int)data2[0]);
                data[1] = (byte)((int)data1[1] & (int)data2[1]);
            }

            setDataFromParameter(Arg2, data1);
        }

        private void NAND(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //NAND Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Logical NAND of Arg1 and Arg2 is stored at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)~((int)data1[1] & (int)data2[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)~((int)data1[0] & (int)data2[0]);
                }
            }
            else
            {
                data[0] = (byte)~((int)data1[0] & (int)data2[0]);
                data[1] = (byte)~((int)data1[1] & (int)data2[1]);
            }

            setDataFromParameter(Arg2, data1);
        }

        private void NOT(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //NOT Arg1
            //----------------------------------
            //Supports all data types EXCEPT literals
            //Inverts the data stored in Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                //Arg1Type == 0x01 ||//Cannot store the result in a literal
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    data[1] = (byte)(~data[1]);
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    data[1] = (byte)(~data[0]);
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data[1] << 8 | data[0]);
                arg1Word = (UInt16)(~arg1Word);
                data = UInt16ToByteArray(arg1Word);
            }

            setDataFromParameter(Arg1, data);
        }

        private void ADD(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ADD Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Stores the sum of Arg1 and Arg2 at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    int result = data1[1] + data2[1];
                    if (result > (int)byte.MaxValue) { result -= (int)byte.MaxValue; }
                    data[1] = (byte)result;
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    int result = data1[0] + data2[0];
                    if (result > (int)byte.MaxValue) { result -= (int)byte.MaxValue; }
                    data[1] = (byte)result;
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);
                UInt16 arg2Word = (UInt16)(data2[0] << 8 | data2[1]);
                UInt16 resultWord = (UInt16)(arg1Word + arg2Word);

                data = UInt16ToByteArray(resultWord);
            }

            setDataFromParameter(Arg2, data);
        }

        private void SUBT(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //SUBT Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Stores (Arg2 - Arg1) at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    int result = data2[1] - data1[1];
                    if (result < 0) { result = (int)byte.MaxValue - result; }
                    data[1] = (byte)result;
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    int result = data2[0] - data1[0];
                    if (result < 0) { result = (int)byte.MaxValue - result; }
                    data[1] = (byte)result;
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);
                UInt16 arg2Word = (UInt16)(data2[0] << 8 | data2[1]);
                UInt16 resultWord = (UInt16)(arg2Word - arg1Word);

                data = UInt16ToByteArray(resultWord);
            }

            setDataFromParameter(Arg2, data);
        }

        private void INCR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ADD Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Stores the sum of Arg1 and Arg2 at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data1;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    int result = data1[1] + 1;
                    if (result > (int)byte.MaxValue) { result -= (int)byte.MaxValue; }
                    data[1] = (byte)result;
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    int result = data1[0] + 1;
                    if (result > (int)byte.MaxValue) { result -= (int)byte.MaxValue; }
                    data[1] = (byte)result;
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);
                UInt16 resultWord = (UInt16)(arg1Word + 1);

                data = UInt16ToByteArray(resultWord);
            }

            setDataFromParameter(Arg1, data);
        }

        private void DECR(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ADD Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Stores the sum of Arg1 and Arg2 at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data1;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    int result = data1[1] - 1;
                    if (result < 0) { result = (int)byte.MaxValue - result; }
                    data[1] = (byte)result;
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    int result = data1[0] - 1;
                    if (result < 0) { result = (int)byte.MaxValue - result; }
                    data[1] = (byte)result;
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);
                UInt16 resultWord = (UInt16)(arg1Word - 1);

                data = UInt16ToByteArray(resultWord);
            }

            setDataFromParameter(Arg1, data);
        }

        private void MULT(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ADD Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Stores the sum of Arg1 and Arg2 at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    int result = data1[1] * data2[1];
                    while (result > (int)byte.MaxValue) { result -= (int)byte.MaxValue; }
                    data[1] = (byte)result;
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    int result = data1[0] * data2[0];
                    while (result > (int)byte.MaxValue) { result -= (int)byte.MaxValue; }
                    data[1] = (byte)result;
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);
                UInt16 arg2Word = (UInt16)(data2[0] << 8 | data2[1]);
                UInt16 resultWord = (UInt16)(arg1Word * arg2Word);

                data = UInt16ToByteArray(resultWord);
            }

            setDataFromParameter(Arg2, data);
        }

        private void DIVI(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //ADD Arg1 Arg2
            //----------------------------------
            //Supports all data types
            //Stores the sum of Arg1 and Arg2 at Arg2
            //==================================
            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;

            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                //Arg2Type == 0x01 ||//Cannot store result in a literal
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }

            byte[] data = data2;
            if (Arg1[0] >= 0x10)
            {
                if (Arg1[0] >= 0x10 && Arg1[0] <= 0x1F)//Lower byte
                {
                    int result = data2[1] / data1[1];
                    data[1] = (byte)result;
                }
                else if (Arg1[0] >= 0x20 && Arg1[0] <= 0x2F)//Upper byte
                {
                    int result = data2[0] / data1[0];
                    data[1] = (byte)result;
                }
            }
            else
            {
                UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);
                UInt16 arg2Word = (UInt16)(data2[0] << 8 | data2[1]);
                UInt16 resultWord = (UInt16)(arg2Word / arg1Word);

                data = UInt16ToByteArray(resultWord);
            }

            setDataFromParameter(Arg2, data);
        }

        private void EXPO(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void GOTO(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //GOTO Arg1
            //----------------------------------
            //Supports all valid data types
            //Copies value from Arg1 into PC
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            PC = (Register)data;
        }

        private void EVAL(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //EVAL Arg1
            //----------------------------------
            //Supports all valid data types
            //Evaluates the data at Arg1 and sets STAT accordingly
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            UInt16 arg1Word = (UInt16)(data[0] << 8 | data[1]);

            if (arg1Word == 0x00)
            {
                STAT.Z = true;
                STAT.L = true;
                STAT.G = false;
            }
            else if (arg1Word > 0x00)
            {
                STAT.Z = false;
                STAT.L = false;
                STAT.G = true;
            }
        }

        private void COMP(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //COMP Arg1 Arg2
            //----------------------------------
            //Supports all valid data types
            //Compares Arg1 against Arg2 and sets STAT accordingly
            //==================================

            byte[] data1 = new byte[2];
            data1[0] = 0x00;
            data1[1] = 0x00;
            byte[] data2 = new byte[2];
            data2[0] = 0x00;
            data2[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data1 = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }
            UInt16 arg1Word = (UInt16)(data1[0] << 8 | data1[1]);

            byte Arg2Type = Arg2[0];
            if (Arg2Type == 0x00 || Arg2Type == 0x10 || Arg2Type == 0x20 ||
                Arg2Type == 0x01 ||
                Arg2Type == 0x02 || Arg2Type == 0x12 || Arg2Type == 0x22 ||
                Arg2Type == 0x03 || Arg2Type == 0x13 || Arg2Type == 0x23 ||
                Arg2Type == 0x04 || Arg2Type == 0x14 || Arg2Type == 0x24 ||
                Arg2Type == 0x05 || Arg2Type == 0x15 || Arg2Type == 0x25)
            {
                data2 = getDataFromParameter(Arg2);
            }
            else { throw new Exception("Unsupported data type"); }
            UInt16 arg2Word = (UInt16)(data2[0] << 8 | data2[1]);

            STAT.Z = (arg1Word == 0);
            STAT.E = (arg1Word == arg2Word);
            STAT.L = (arg1Word <= arg2Word);
            STAT.G = (arg1Word > arg2Word);
        }

        private void JMPZ(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //JMPZ Arg1
            //----------------------------------
            //Supports all valid data types
            //If the Z flag is set, PC is set to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (STAT.Z)
            {
                PC = (Register)data;
            }
        }

        private void JMGZ(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //JMPZ Arg1
            //----------------------------------
            //Supports all valid data types
            //If the Z flag is not set, PC is set to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (!STAT.Z)
            {
                PC = (Register)data;
            }
        }

        private void JMLZ(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //JMLZ Arg1
            //----------------------------------
            //Supports all valid data types
            //If the N flag is set, PC is set to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (STAT.N)
            {
                PC = (Register)data;
            }
        }

        private void GSUB(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //GSUB Arg1
            //----------------------------------
            //Supports all valid data types
            //Stores PC in SUBR, sets PC to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            UInt16 arg1Word = (UInt16)(data[0] << 8 | data[1]);
            SUBR = (Register)PC.ToInt();
            PC = (Register)arg1Word;
        }

        private void RTRN(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //RTRN
            //----------------------------------
            //Sets PC to the address after SUBR
            //==================================
            PC = (Register)(SUBR.ToInt() + 1);
        }

        private void JMPE(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //JMPE Arg1
            //----------------------------------
            //Supports all valid data types
            //If the E flag is set, PC is set to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (STAT.E)
            {
                PC = (Register)data;
            }
        }

        private void JMPG(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //JMPG Arg1
            //----------------------------------
            //Supports all valid data types
            //If the G flag is set, PC is set to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (STAT.G)
            {
                PC = (Register)data;
            }
        }

        private void JMPL(byte[] Arg1, byte[] Arg2)
        {
            //==================================
            //JMPL Arg1
            //----------------------------------
            //Supports all valid data types
            //If the L flag is set but not E, PC is set to Arg1
            //==================================

            byte[] data = new byte[2];
            data[0] = 0x00;
            data[1] = 0x00;

            byte Arg1Type = Arg1[0];
            if (Arg1Type == 0x00 || Arg1Type == 0x10 || Arg1Type == 0x20 ||
                Arg1Type == 0x01 ||
                Arg1Type == 0x02 || Arg1Type == 0x12 || Arg1Type == 0x22 ||
                Arg1Type == 0x03 || Arg1Type == 0x13 || Arg1Type == 0x23 ||
                Arg1Type == 0x04 || Arg1Type == 0x14 || Arg1Type == 0x24 ||
                Arg1Type == 0x05 || Arg1Type == 0x15 || Arg1Type == 0x25)
            {
                data = getDataFromParameter(Arg1);
            }
            else { throw new Exception("Unsupported data type"); }

            if (STAT.L && !STAT.E)
            {
                PC = (Register)data;
            }
        }

        private void ENQU(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void DEQU(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void PUSH(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void POP(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void TXD0(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void RXD0(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void TXD1(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void RXD1(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void ROMR(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        private void ROMW(byte[] Arg1, byte[] Arg2)
        {
            throw new NotImplementedException();
        }

        #endregion Operations

        private void setDataFromParameter(byte[] _parameter, byte[] _data)
        {
            //Takes in a 3-byte parameter and 2-byte data
            //Stores data where the parameter specifies
            //ONLY considers Upper/Lower byte mode of destination.
            //For byte modes, only stores _data[1]

            int byteMode = 0;
            if (_parameter[0] >= 0x10)
            {
                if (_parameter[0] >= 0x10 && _parameter[0] <= 0x1F)//Lower byte
                {
                    byteMode = 1;
                }
                else if (_parameter[0] >= 0x20 && _parameter[0] <= 0x2F)//Upper byte
                {
                    byteMode = 2;
                }
            }

            if (_parameter[0] == 0x00 || _parameter[0] == 0x10 || _parameter[0] == 0x20)//Register
            {
                setRegisterFromID(_parameter[2], _data, byteMode);
            }
            else if (_parameter[0] == 0x01)//Literal, invalid
            {
                throw new Exception("\nCannot store data in a literal\n");
            }
            else if (_parameter[0] == 0x02 || _parameter[0] == 0x12 || _parameter[0] == 0x22)//Absolute RAM
            {
                int a = (int)(((int)_parameter[1] + 0xFF) + ((int)_parameter[2]));

                if (byteMode == 0) //I use copy/paste this same if/else block several times. Maybe it should be a method.
                {
                    RAM[a] = _data[1];
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[0];
                    }
                }
                else if (byteMode == 1)
                {
                    RAM[a] = _data[1];
                }
                else if (byteMode == 2)
                {
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[1];
                    }
                }
            }
            else if (_parameter[0] == 0x03 || _parameter[0] == 0x13 || _parameter[0] == 0x23)//Indirect RAM
            {
                int a = (int)(((int)_parameter[1] + 0xFF) + ((int)_parameter[2]));
                a += RREF.ToInt();
                if (byteMode == 0)
                {
                    RAM[a] = _data[1];
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[0];
                    }
                }
                else if (byteMode == 1)
                {
                    RAM[a] = _data[1];
                }
                else if (byteMode == 2)
                {
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[1];
                    }
                }
            }
            else if (_parameter[0] == 0x04 || _parameter[0] == 0x14 || _parameter[0] == 0x24)//Direct address stored in a register
            {
                byte[] data = getRegisterFromID(_parameter[2]);
                int a = (int)(((int)data[0] + 0xFF) + ((int)data[1]));
                if (byteMode == 0)
                {
                    RAM[a] = _data[1];
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[0];
                    }
                }
                else if (byteMode == 1)
                {
                    RAM[a] = _data[1];
                }
                else if (byteMode == 2)
                {
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[1];
                    }
                }
            }
            else if (_parameter[0] == 0x05 || _parameter[0] == 0x15 || _parameter[0] == 0x25)//Indirect RAM stored in a register
            {
                byte[] data = getRegisterFromID(_parameter[2]);
                int a = (int)(((int)data[0] + 0xFF) + ((int)data[1]));
                a += RREF.ToInt();

                if (byteMode == 0)
                {
                    RAM[a] = _data[1];
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[0];
                    }
                }
                else if (byteMode == 1)
                {
                    RAM[a] = _data[1];
                }
                else if (byteMode == 2)
                {
                    a++;
                    if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around
                    if (a < RAM.Size)
                    {
                        RAM[a] = _data[1];
                    }
                }
            }
        }

        private byte[] getDataFromParameter(byte[] _parameter)
        {
            //Takes in a 3-byte parameter and returns the 2-byte data stored at that location/register
            byte[] result = new byte[2];

            if (_parameter[0] == 0x00 || _parameter[0] == 0x10 || _parameter[0] == 0x20)//Register
            {
                result = getRegisterFromID(_parameter[2]);
            }
            else if (_parameter[0] == 0x01)//Literal
            {
                result[0] = _parameter[1];
                result[1] = _parameter[2];
            }
            else if (_parameter[0] == 0x02 || _parameter[0] == 0x12 || _parameter[0] == 0x22)//Absolute RAM
            {
                int a = (int)(((int)_parameter[1] + 0xFF) + ((int)_parameter[2]));
                result[0] = RAM[a];
                a++;
                if (a < RAM.Size)
                {
                    result[1] = RAM[a];
                }
            }
            else if (_parameter[0] == 0x03 || _parameter[0] == 0x13 || _parameter[0] == 0x23)//Indirect RAM
            {
                int a = (int)(((int)_parameter[1] + 0xFF) + ((int)_parameter[2]));
                a += RREF.ToInt();

                if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around. We could throw an exception but it is a technically valid instruction.
                                                     //If you overflow your offset RAM address, it's your own darn fault!
                result[0] = RAM[a];
                a++;
                if (a < RAM.Size) { result[1] = RAM[a]; }
            }
            else if (_parameter[0] == 0x04 || _parameter[0] == 0x14 || _parameter[0] == 0x24)//Direct address stored in a register
            {
                byte[] data = getRegisterFromID(_parameter[2]);
                int a = (int)(((int)data[0] + 0xFF) + ((int)data[1]));

                result[0] = RAM[a];
                a++;
                if (a < RAM.Size)
                {
                    result[1] = RAM[a];
                }
            }
            else if (_parameter[0] == 0x05 || _parameter[0] == 0x15 || _parameter[0] == 0x25)//Indirect RAM stored in a register
            {
                byte[] data = getRegisterFromID(_parameter[2]);
                int a = (int)(((int)data[0] + 0xFF) + ((int)data[1]));
                a += RREF.ToInt();

                if (a >= RAM.Size) { a -= RAM.Size; }//Wrap around. We could throw an exception but it is a technically valid instruction.
                                                     //If you overflow your offset RAM address, it's your own darn fault!
                result[0] = RAM[a];
                a++;
                if (a < RAM.Size) { result[1] = RAM[a]; }
            }

            //Check for Byte modes
            if (_parameter[0] >= 0x10)
            {
                if (_parameter[0] >= 0x10 && _parameter[0] <= 0x1F)//Lower byte
                {
                    result[0] = 0x00;
                }
                else if (_parameter[0] >= 0x20 && _parameter[0] <= 0x2F)//Upper byte
                {
                    result[0] = result[1];
                    result[1] = 0x00;
                }
            }

            return result;
        }

        private void setRegisterFromID(byte id, byte[] value, int mode)
        {
            //Mode:
            // 0:   16-bit word
            // 1:   Lower byte, value[1]
            // 2:   Upper byte, value[0]

            switch (id)
            {
                case 0x00:
                    if (mode == 0) { PC = (Register)value; }
                    else if (mode == 1) { PC.LowerByte = value[1]; }
                    else if (mode == 2) { PC.UpperByte = value[1]; }
                    break;

                case 0x01:
                    if (mode == 0) { STAT = (StatusRegister)value; }
                    else if (mode == 1) { STAT.LowerByte = value[1]; }
                    else if (mode == 2) { STAT.UpperByte = value[1]; }
                    break;

                case 0x02:
                    if (mode == 0) { SUBR = (Register)value; }
                    else if (mode == 1) { SUBR.LowerByte = value[1]; }
                    else if (mode == 2) { SUBR.UpperByte = value[1]; }
                    break;

                case 0x03:
                    if (mode == 0) { PSTR = (Register)value; }
                    else if (mode == 1) { PSTR.LowerByte = value[1]; }
                    else if (mode == 2) { PSTR.UpperByte = value[1]; }
                    break;

                case 0x04:
                    if (mode == 0) { PEND = (Register)value; }
                    else if (mode == 1) { PEND.LowerByte = value[1]; }
                    else if (mode == 2) { PEND.UpperByte = value[1]; }
                    break;

                case 0x05:
                    throw new Exception("\nCannot store data in RAND\n");
                //break;

                case 0x06:
                    if (mode == 0) { RREF = (Register)value; }
                    else if (mode == 1) { RREF.LowerByte = value[1]; }
                    else if (mode == 2) { RREF.UpperByte = value[1]; }
                    break;

                case 0x07:
                    if (mode == 0) { PAGE = (Register)value; }
                    else if (mode == 1) { PAGE.LowerByte = value[1]; }
                    else if (mode == 2) { PAGE.UpperByte = value[1]; }
                    break;

                case 0x08:
                    if (mode == 0) { MEMS = (Register)value; }
                    else if (mode == 1) { MEMS.LowerByte = value[1]; }
                    else if (mode == 2) { MEMS.UpperByte = value[1]; }
                    break;

                case 0x09:
                    if (mode == 0) { PEEK = (Register)value; }
                    else if (mode == 1) { PEEK.LowerByte = value[1]; }
                    else if (mode == 2) { PEEK.UpperByte = value[1]; }
                    break;

                case 0xF0:
                    if (mode == 0) { USR0 = (Register)value; }
                    else if (mode == 1) { USR0.LowerByte = value[1]; }
                    else if (mode == 2) { USR0.UpperByte = value[1]; }
                    break;

                case 0xF1:
                    if (mode == 0) { USR1 = (Register)value; }
                    else if (mode == 1) { USR1.LowerByte = value[1]; }
                    else if (mode == 2) { USR1.UpperByte = value[1]; }
                    break;

                case 0xF2:
                    if (mode == 0) { USR2 = (Register)value; }
                    else if (mode == 1) { USR2.LowerByte = value[1]; }
                    else if (mode == 2) { USR2.UpperByte = value[1]; }
                    break;

                case 0xF3:
                    if (mode == 0) { USR3 = (Register)value; }
                    else if (mode == 1) { USR3.LowerByte = value[1]; }
                    else if (mode == 2) { USR3.UpperByte = value[1]; }
                    break;

                case 0xF4:
                    if (mode == 0) { USR4 = (Register)value; }
                    else if (mode == 1) { USR4.LowerByte = value[1]; }
                    else if (mode == 2) { USR4.UpperByte = value[1]; }
                    break;

                case 0xF5:
                    if (mode == 0) { USR5 = (Register)value; }
                    else if (mode == 1) { USR5.LowerByte = value[1]; }
                    else if (mode == 2) { USR5.UpperByte = value[1]; }
                    break;

                case 0xF6:
                    if (mode == 0) { USR6 = (Register)value; }
                    else if (mode == 1) { USR6.LowerByte = value[1]; }
                    else if (mode == 2) { USR6.UpperByte = value[1]; }
                    break;

                case 0xF7:
                    if (mode == 0) { USR7 = (Register)value; }
                    else if (mode == 1) { USR7.LowerByte = value[1]; }
                    else if (mode == 2) { USR7.UpperByte = value[1]; }
                    break;

                case 0xF8:
                    if (mode == 0) { USR8 = (Register)value; }
                    else if (mode == 1) { USR8.LowerByte = value[1]; }
                    else if (mode == 2) { USR8.UpperByte = value[1]; }
                    break;

                case 0xF9:
                    if (mode == 0) { USR9 = (Register)value; }
                    else if (mode == 1) { USR9.LowerByte = value[1]; }
                    else if (mode == 2) { USR9.UpperByte = value[1]; }
                    break;

                case 0xFA:
                    if (mode == 0) { USRA = (Register)value; }
                    else if (mode == 1) { USRA.LowerByte = value[1]; }
                    else if (mode == 2) { USRA.UpperByte = value[1]; }
                    break;

                case 0xFB:
                    if (mode == 0) { USRB = (Register)value; }
                    else if (mode == 1) { USRB.LowerByte = value[1]; }
                    else if (mode == 2) { USRB.UpperByte = value[1]; }
                    break;

                case 0xFC:
                    if (mode == 0) { USRC = (Register)value; }
                    else if (mode == 1) { USRC.LowerByte = value[1]; }
                    else if (mode == 2) { USRC.UpperByte = value[1]; }
                    break;

                case 0xFD:
                    if (mode == 0) { USRD = (Register)value; }
                    else if (mode == 1) { USRD.LowerByte = value[1]; }
                    else if (mode == 2) { USRD.UpperByte = value[1]; }
                    break;

                case 0xFE:
                    if (mode == 0) { USRE = (Register)value; }
                    else if (mode == 1) { USRE.LowerByte = value[1]; }
                    else if (mode == 2) { USRE.UpperByte = value[1]; }
                    break;

                case 0xFF:
                    if (mode == 0) { USRF = (Register)value; }
                    else if (mode == 1) { USRF.LowerByte = value[1]; }
                    else if (mode == 2) { USRF.UpperByte = value[1]; }
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
                    result[0] = RAND.LowerByte;
                    result[1] = RAND.UpperByte;
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

        public int LoadROM(int offset, string path)
        {
            if (File.Exists(path))
            {
                int length = 0;
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    length = (int)reader.BaseStream.Length;
                    byte[] chunk = new byte[8];
                    int n = 0;
                    for (int i = 0; i < length; i++)
                    {
                        ROM[i + offset] = reader.ReadByte();
                        chunk[n] = ROM[i + offset];
                        n++;
                        if (n >= 8)
                        {
                            n = 0;
                            Message = "Loaded " + ASM.ByteArrayToString(chunk);
                        }
                    }
                }
                return length;
            }

            return -1;
        }

        public void LoadROM(string path)
        {
            LoadROM(0, path);
        }

        private static byte[] UInt16ToByteArray(UInt16 b)
        {
            byte[] result = new byte[2];

            result = BitConverter.GetBytes(b);

            return result;
        }
    }
}