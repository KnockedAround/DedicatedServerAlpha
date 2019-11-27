using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DedicatedServer.Alpha
{
    public class ByteBuffer : IDisposable
    {
        private List<byte> _buffer; // Holds packet information
        private byte[] _bufferRead; // Reads packet information
        private int _bufferReadPosition; // The position of the buffer read stream
        private bool _bufferUpdated = false; // Has the Buffer been updated.
        private bool disposedValue = false; // Garbage Collection check for should this obj dispose

        //constructor
        public ByteBuffer()
        {
            _buffer = new List<byte>();
            _bufferReadPosition = 0; // ever instance starts at 0
        }

        // Get buffer read position
        public int GetBufferReadPosition()
        {
            return _bufferReadPosition;
        }
        // Convert list to byte array
        public byte[] ToByteArray()
        {
            return _buffer.ToArray();
        }
        // Count the length of the buffer
        public int Count()
        {
           return _buffer.Count();
        }
        // Get the remaining length of the buffer at the current read position
        public int GetRemainingLength()
        {
            return Count() - _bufferReadPosition;
        }
        // Clear the buffer when processing finished
        public void Clear()
        {
            _buffer.Clear();
            _bufferReadPosition = 0;
        }

        //
        // WRITE TO BUFFER 
        //

        // Add byte to buffer
        public void WriteByte(byte input)
        {
            _buffer.Add(input);
            _bufferUpdated = true;
        }
        // Add byte array to buffer
        public void WriteBytes(byte[] input)
        {
            _buffer.AddRange(input);
            _bufferUpdated = true;
        }
        // Add short to buffer
        public void WriteShort(short input)
        {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _bufferUpdated = true;
        }
        // Add int to buffer
        public void WriteInteger(int input)
        {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _bufferUpdated = true;
        }
        // Add long to buffer
        public void WriteLong(long input)
        {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _bufferUpdated = true;
        }
        // Add float to buffer
        public void WriteFloat(float input)
        {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _bufferUpdated = true;
        }
        // Add boolean to buffer
        public void WriteBoolean(bool input)
        {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _bufferUpdated = true;
        }
        // Add string to buffer
        public void WriteString(string input)
        {
            _buffer.AddRange(BitConverter.GetBytes(input.Length));
            _buffer.AddRange(Encoding.ASCII.GetBytes(input));
            _bufferUpdated = true;
        }

        //
        // READ FROM BUFFER
        ///
        public byte ReadByte(bool Peek = true)
        {
            if(_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                byte value = _bufferRead[_bufferReadPosition];

                if(Peek & _buffer.Count > _bufferReadPosition)
                {
                    _bufferReadPosition++;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'byte' on ByteBuffer.ReadByte()...");
            }
        }
        public byte[] ReadBytes(int length, bool Peek = true)
        {
            if (_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                byte[] value = _buffer.GetRange(_bufferReadPosition, length).ToArray();

                if (Peek )
                {
                    _bufferReadPosition += length;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'byte[]' on ByteBuffer.ReadBytes()...");
            }
        }
        public short ReadShort(bool Peek = true)
        {
            if (_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                short value = BitConverter.ToInt16(_bufferRead, _bufferReadPosition);

                if (Peek & _buffer.Count > _bufferReadPosition)
                {
                    _bufferReadPosition += 2;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'short' on ByteBuffer.ReadShort()...");
            }
        }
        public int ReadInteger(bool Peek = true)
        {
            if (_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                int value = BitConverter.ToInt32(_bufferRead, _bufferReadPosition);

                if (Peek & _buffer.Count > _bufferReadPosition)
                {
                    _bufferReadPosition += 4;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'int' on ByteBuffer.ReadInteger()");
            }
        }
        public long ReadLong(bool Peek = true)
        {
            if (_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                long value = BitConverter.ToInt64(_bufferRead, _bufferReadPosition);

                if (Peek & _buffer.Count > _bufferReadPosition)
                {
                    _bufferReadPosition += 8;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'long' on ByteBuffer.ReadLong()...");
            }
        }
        public float ReadFloat(bool Peek = true)
        {
            if (_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                float value = BitConverter.ToSingle(_bufferRead, _bufferReadPosition);

                if (Peek & _buffer.Count > _bufferReadPosition)
                {
                    _bufferReadPosition += 4;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'float' on ByteBuffer.ReadFloat()...");
            }
        }
        public bool ReadBoolean(bool Peek = true)
        {
            if (_buffer.Count() > _bufferReadPosition)
            {
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                bool value = BitConverter.ToBoolean(_bufferRead, _bufferReadPosition);

                if (Peek & _buffer.Count > _bufferReadPosition)
                {
                    _bufferReadPosition++;
                }

                return value;
            }
            else
            {
                throw new Exception("Incorrect type: expected a 'bool' on ByteBuffer.ReadBoolean()...");
            }
        }
        public string ReadString(bool Peek = true)
        {
            try
            {
                int length = ReadInteger(true);
                if (_bufferUpdated)
                {
                    _bufferRead = _buffer.ToArray();
                    _bufferUpdated = false;
                }

                string value = Encoding.ASCII.GetString(_bufferRead, _bufferReadPosition, length);

                if (Peek & _buffer.Count > _bufferReadPosition)
                {
                    if (value.Length > 0)
                    {
                        _bufferReadPosition = length;
                    }
                }

                return value;
            }
            catch (Exception)
            {

                throw new Exception("Incorrect type: expected a 'string' on ByteBuffer.StringRead()...");
            }
           
        }

     
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _buffer.Clear();
                    _bufferReadPosition = 0;
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
