using SG16;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class Peripheral
    {
        protected Queue<byte> InputBuffer;
        protected Queue<byte> OutputBuffer;
        public int InputAddress = -1;
        public int OutputAddress = -1;
        public int ControlAddress = -1;
        protected Memory ramReference;

        public Peripheral()
        {
            InputBuffer = new Queue<byte>();
            OutputBuffer = new Queue<byte>();
        }

        public Peripheral(int _inputAddress, int _outputAddress, int _controlAddress, Memory _ramReference)
        {
            InputBuffer = new Queue<byte>();
            OutputBuffer = new Queue<byte>();
            InputAddress = _inputAddress;
            OutputAddress = _outputAddress;
            ramReference = _ramReference;
        }

        public void Attach(int _inputAddress, int _outputAddress, int _controlAddress, Memory _ramReference)
        {
            InputAddress = _inputAddress;
            OutputAddress = _outputAddress;
            ramReference = _ramReference;
        }

        public long Tick()
        {
            Stopwatch sw = Stopwatch.StartNew();

            //Execute Instruction
            Function();

            long result = sw.ElapsedMilliseconds;
            sw.Stop();
            return result;
        }

        virtual public void Function()
        {
            if (InputAddress >= 0)
            {
                InputBuffer.Enqueue(ramReference[InputAddress]);
            }

            //Peripheral-specific logic goes here
            //This is a simple loopback buffer that moves bytes from the input
            //address to the output address
            OutputBuffer.Enqueue(InputBuffer.Dequeue());

            if (OutputAddress >= 0)
            {
                ramReference[OutputAddress] = OutputBuffer.Dequeue();
            }
        }
    }
}