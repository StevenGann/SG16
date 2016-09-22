using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG16
{
    public class Terminal : Peripheral
    {
        public string Text = "";

        new public void Attach(int _inputAddress, int _outputAddress, int _controlAddress, Memory _ramReference)
        {
            InputAddress = _inputAddress;
            OutputAddress = _outputAddress;
            ramReference = _ramReference;
        }

        public void Feed(string _input)
        {
            foreach (char c in _input)
            {
                OutputBuffer.Enqueue(Convert.ToByte(c));
            }
        }

        override public void Function()
        {
            if (InputAddress >= 0)//If the Input has been attached
            {
                if (ramReference[InputAddress] != 0x00)//If there's something other than a NULL
                {
                    InputBuffer.Enqueue(ramReference[InputAddress]);
                    ramReference[InputAddress] = 0x00;
                }
            }

            while (InputBuffer.Count > 0)
            {
                byte newByte = InputBuffer.Dequeue();

                if (newByte == 0x0C)// Form Feed character, clears the terminal
                {
                    Text = "";
                }
                else if (newByte == 0x10 || newByte == 0x7F)//Delete, Backspace
                {
                    Text = Text.Substring(0, Text.Length - 1);
                }
                else if (newByte == 0x0A || newByte == 0x0D)//Newline, Carriage Return
                {
                    Text += Environment.NewLine; //You'd expect this to just be /n. You'd be wrong.
                }
                else if (newByte >= 0x20 && newByte <= 0x7E)//Printable chars from Space to ~
                {
                    Text += Convert.ToChar(newByte);
                }
                else if (newByte >= 0x80 && newByte <= 0xFE)//Extended ASCII
                {
                    Text += Convert.ToChar(newByte);
                }
                else//Unrecognized character, probably not printable
                {
                    Text += "?";
                }
            }

            if (OutputAddress >= 0)//If the Output has been attached
            {
                if (OutputBuffer.Count > 0)
                {
                    if (ramReference[OutputAddress] == 0x00)//If the output has been reset to NULL
                    {
                        ramReference[OutputAddress] = OutputBuffer.Dequeue();
                    }
                }
            }
        }
    }
}