#region LICENCE
// Copyright (c) Jose Luis Herrejon Diaz
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Buffers;
using CommunityToolkit.HighPerformance.Buffers;

namespace RakNet.Binary.Writer;

/// <summary>
/// Provides static methods for writing primitive data types to a binary buffer.
/// This class is designed for efficient data serialization of fundamental data types.
/// </summary>
internal static class BinaryPrimitiveWriter
{
    public static void WriteByte(MemoryBufferWriter<byte> buffer, byte value)
    {
        Span<byte> bytes = stackalloc byte[BinaryDataSize.Int8BinarySize];
        bytes[0] = value;
        
        buffer.Write(bytes);
    }

    public static void WriteBool(MemoryBufferWriter<byte> buffer, bool value)
    {
        WriteByte(buffer, (byte)(value ? 1 : 0));
    }

    public static void WriteInt16(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, short value)
    {
        Span<byte> bytes = stackalloc byte[BinaryDataSize.Int16BinarySize];
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                bytes[0] = (byte)value;
                bytes[1] = (byte)(value >> 8);
                break;
            case BinaryEncoding.BigEndian:
                bytes[0] = (byte)(value >> 8);
                bytes[1] = (byte)value;
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding}");
        }
        
        buffer.Write(bytes);
    }

    public static void WriteUInt16(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, ushort value)
    {
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                WriteInt16(BinaryEncoding.LittleEndian, buffer, (short)(value & short.MaxValue));
                break;
            case BinaryEncoding.BigEndian:
                WriteInt16(BinaryEncoding.BigEndian, buffer, (short)(value & short.MaxValue));
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}");
        }
    }

    public static void WriteInt32(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, int value)
    {
        Span<byte> bytes = stackalloc byte[BinaryDataSize.Int32BinarySize];
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                bytes[0] = (byte)value;
                bytes[1] = (byte)(value >> 8);
                bytes[2] = (byte)(value >> 16);
                bytes[3] = (byte)(value >> 24);
                break;
            case BinaryEncoding.BigEndian:
                bytes[0] = (byte)(value >> 24);
                bytes[1] = (byte)(value >> 16);
                bytes[2] = (byte)(value >> 8);
                bytes[3] = (byte)value;
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding}");
        }
        
        buffer.Write(bytes);
    }

    public static void WriteUInt32(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, uint value)
    {
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                WriteInt32(BinaryEncoding.LittleEndian, buffer, (int)(value & int.MaxValue));
                break;
            case BinaryEncoding.BigEndian:
                WriteInt32(BinaryEncoding.BigEndian, buffer, (int)(value & int.MaxValue));
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}");
        }
    }

    public static void WriteInt64(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, long value)
    {
        Span<byte> bytes = stackalloc byte[BinaryDataSize.Int64BinarySize];
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                bytes[0] = (byte)value;
                bytes[1] = (byte)(value >> 8);
                bytes[2] = (byte)(value >> 16);
                bytes[3] = (byte)(value >> 24);
                bytes[4] = (byte)(value >> 32);
                bytes[5] = (byte)(value >> 40);
                bytes[6] = (byte)(value >> 48);
                bytes[7] = (byte)(value >> 56);
                break;
            case BinaryEncoding.BigEndian:
                bytes[0] = (byte)(value >> 56);
                bytes[1] = (byte)(value >> 48);
                bytes[2] = (byte)(value >> 40);
                bytes[3] = (byte)(value >> 32);
                bytes[4] = (byte)(value >> 24);
                bytes[5] = (byte)(value >> 16);
                bytes[6] = (byte)(value >> 8);
                bytes[7] = (byte)value;
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding}");
        }
        
        buffer.Write(bytes);
    }

    public static void WriteUInt64(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, ulong value)
    {
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                WriteInt64(BinaryEncoding.LittleEndian, buffer, (long)(value & long.MaxValue));
                break;
            case BinaryEncoding.BigEndian:
                WriteInt64(BinaryEncoding.BigEndian, buffer, (long)(value & long.MaxValue));
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}");
        }
    }
}