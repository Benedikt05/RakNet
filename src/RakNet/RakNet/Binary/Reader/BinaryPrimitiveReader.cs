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

namespace RakNet.Binary.Reader;

/// <summary>
/// Provides static methods for reading primitive binary data types,
/// Designed for reading specific data types or custom binary structures efficiently.
/// </summary>
internal static class BinaryPrimitiveReader
{
    public static byte ReadByte(ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryDataSize.Int8BinarySize + 1);
        
        return buffer[position++];
    }

    public static bool ReadBoolean(ref int position, ReadOnlySpan<byte> buffer)
    {
        return ReadByte(ref position, buffer) == 1;
    }

    public static short ReadInt16(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryDataSize.Int16BinarySize + 1);
        
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (short)(buffer[position++] & 0xFF | (buffer[position++] & 0xFF) << 8),
            BinaryEncoding.BigEndian => (short) ((buffer[position++] & 0xFF) << 8 | buffer[position++] & 0xFF),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}")
        };
    }

    public static ushort ReadUInt16(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (ushort)(ReadInt16(BinaryEncoding.LittleEndian, ref position, buffer) & short.MaxValue),
            BinaryEncoding.BigEndian => (ushort)(ReadInt16(BinaryEncoding.BigEndian, ref position, buffer) & short.MaxValue),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}")
        };
    }
    
    public static int ReadInt32(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryDataSize.Int32BinarySize + 1);
        
        return encoding switch
        {
            BinaryEncoding.LittleEndian => buffer[position++] & 0xFF | (buffer[position++] & 0xFF) << 8  | 
                                           (buffer[position++] & 0xFF) << 16 | (buffer[position++] & 0xFF) << 24,
            BinaryEncoding.BigEndian => (buffer[position++] & 0xFF) << 24 | (buffer[position++] & 0xFF) << 16 | 
                                        (buffer[position++] & 0xFF) << 8  | buffer[position++] & 0xFF,
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}")
        };
    }

    public static uint ReadUInt32(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (uint)(ReadInt32(BinaryEncoding.LittleEndian, ref position, buffer) & int.MaxValue),
            BinaryEncoding.BigEndian => (uint)(ReadInt32(BinaryEncoding.BigEndian, ref position, buffer) & int.MaxValue),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}")
        };
    }
    
    public static long ReadInt64(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryDataSize.Int64BinarySize + 1);
        
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (long)buffer[position++] & 0xFF | (long)(buffer[position++] & 0xFF) << 8  |
                                           (long)(buffer[position++] & 0xFF) << 16 | (long)(buffer[position++] & 0xFF) << 24 | 
                                           (long)(buffer[position++] & 0xFF) << 32 | (long)(buffer[position++] & 0xFF) << 40 | 
                                           (long)(buffer[position++] & 0xFF) << 48 | (long)(buffer[position++] & 0xFF) << 56,
            BinaryEncoding.BigEndian => (long)(buffer[position++] & 0xFF) << 56 | (long)(buffer[position++] & 0xFF) << 48 |
                                        (long)(buffer[position++] & 0xFF) << 40 | (long)(buffer[position++] & 0xFF) << 32 | 
                                        (long)(buffer[position++] & 0xFF) << 24 | (long)(buffer[position++] & 0xFF) << 16 | 
                                        (long)(buffer[position++] & 0xFF) << 8  | (long)buffer[position++] & 0xFF,
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}")
        };
    }

    public static ulong ReadUInt64(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (ulong)(ReadInt64(BinaryEncoding.LittleEndian, ref position, buffer) & long.MaxValue),
            BinaryEncoding.BigEndian => (ulong)(ReadInt64(BinaryEncoding.BigEndian, ref position, buffer) & long.MaxValue),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}")
        };
    }
}