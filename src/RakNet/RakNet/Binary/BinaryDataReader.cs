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

using System.Runtime.CompilerServices;

namespace RakNet.Binary;

internal static class BinaryDataReader
{
    public static byte ReadByte(ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryHelper.Int8BinarySize + 1);
        
        return buffer[position++];
    }

    public static bool ReadBoolean(ref int position, ReadOnlySpan<byte> buffer)
    {
        return ReadByte(ref position, buffer) == 1;
    }

    public static short ReadInt16(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryHelper.Int16BinarySize + 1);
        
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (short)(buffer[position++] & 0xFF | (buffer[position++] & 0xFF) << 8),
            BinaryEncoding.BigEndian => (short) ((buffer[position++] & 0xFF) << 8 | buffer[position++] & 0xFF),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}"),
        };
    }

    public static ushort ReadUInt16(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (ushort)(ReadInt16(BinaryEncoding.LittleEndian, ref position, buffer) & 0xFFFF),
            BinaryEncoding.BigEndian => (ushort)(ReadInt16(BinaryEncoding.BigEndian, ref position, buffer) & 0xFFFF),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}"),
        };
    }

    public static int ReadInt24(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryHelper.Int24BinarySize + 1);
        
        var value = encoding == BinaryEncoding.LittleEndian
            ? buffer[position++] | (buffer[position++] << 8) | (buffer[position++] << 16)
            : (buffer[position++] << 16) | (buffer[position++] << 8) | buffer[position++];
        
        if ((value & 0x800000) != 0)
        {
            // Extends the sign by filling the upper 8 bits
            value |= unchecked((int)0xFF000000);
        }

        return value;
    }

    public static uint ReadUInt24(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryHelper.Int24BinarySize + 1);
        
        return encoding == BinaryEncoding.LittleEndian
            ? (uint)(buffer[position++] | (buffer[position++] << 8) | (buffer[position++] << 16))
            : (uint)((buffer[0] << position++) | (buffer[position++] << 8) | buffer[position++]);
    }

    public static int ReadInt32(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryHelper.Int32BinarySize + 1);
        
        return encoding switch
        {
            BinaryEncoding.LittleEndian => buffer[position++] & 0xFF | 
                                           (buffer[position++] & 0xFF) << 8  | 
                                           (buffer[position++] & 0xFF) << 16 | 
                                           (buffer[position++] & 0xFF) << 24,
            BinaryEncoding.BigEndian => (buffer[position++] & 0xFF) << 24 | 
                                        (buffer[position++] & 0xFF) << 16 | 
                                        (buffer[position++] & 0xFF) << 8  | 
                                        buffer[position++] & 0xFF,
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}"),
        };
    }

    public static uint ReadUInt32(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (uint)(ReadInt32(BinaryEncoding.LittleEndian, ref position, buffer) & 0xFFFFFFFF),
            BinaryEncoding.BigEndian => (uint)(ReadInt32(BinaryEncoding.BigEndian, ref position, buffer) & 0xFFFFFFFF),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}"),
        };
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadVarInt32(ref int position, ReadOnlySpan<byte> buffer)
    {
        var result = ReadUnsignedVarInt32(ref position, buffer);
        return ((((result << 31) >> 31) ^ result) >> 1) ^ (result & (1 << 31));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadUnsignedVarInt32(ref int position, ReadOnlySpan<byte> buffer)
    {
        var result = 0;
        var size = 0;

        byte head;
        
        while (true) {
            head = ReadByte(ref position, buffer);
            result |= (head & 0x7F) << (7 * size++);

            if ((head & 0x80) != 0x80 || size >= 6) {
                break;
            }
        }

        return result;
    }

    public static long ReadInt64(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryHelper.Int64BinarySize + 1);
        
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (long)buffer[position++] & 0xFF        |
                                           (long)(buffer[position++] & 0xFF) << 8  |
                                           (long)(buffer[position++] & 0xFF) << 16 |
                                           (long)(buffer[position++] & 0xFF) << 24 | 
                                           (long)(buffer[position++] & 0xFF) << 32 | 
                                           (long)(buffer[position++] & 0xFF) << 40 | 
                                           (long)(buffer[position++] & 0xFF) << 48 | 
                                           (long)(buffer[position++] & 0xFF) << 56,
            BinaryEncoding.BigEndian => (long)(buffer[position++] & 0xFF) << 56 |
                                        (long)(buffer[position++] & 0xFF) << 48 |
                                        (long)(buffer[position++] & 0xFF) << 40 |
                                        (long)(buffer[position++] & 0xFF) << 32 | 
                                        (long)(buffer[position++] & 0xFF) << 24 | 
                                        (long)(buffer[position++] & 0xFF) << 16 | 
                                        (long)(buffer[position++] & 0xFF) << 8  | 
                                        (long)buffer[position++] & 0xFF,
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}"),
        };
    }

    public static ulong ReadUInt64(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        return encoding switch
        {
            BinaryEncoding.LittleEndian => (ulong)(ReadInt64(BinaryEncoding.LittleEndian, ref position, buffer) & 0x7FFFFFFFFFFFFFFF),
            BinaryEncoding.BigEndian => (ulong)(ReadInt64(BinaryEncoding.BigEndian, ref position, buffer) & 0x7FFFFFFFFFFFFFFF),
            _ => throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding.ToString()}"),
        };
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadVarInt64(ref int position, ReadOnlySpan<byte> buffer)
    {
        var result = ReadUnsignedVarInt64(ref position, buffer);
        return ((((result << 63) >> 63) ^ result) >> 1) ^ (result & (1L << 63));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadUnsignedVarInt64(ref int position, ReadOnlySpan<byte> buffer)
    {
        var value = 0L;
        var size = 0;
        
        byte head;
        
        while (((head = ReadByte(ref position, buffer)) & 0x80) != 0) {
            value |= (long) (head & 0x7F) << size++ * 7;
            if (size > 10) throw new OverflowException("VarLong too big");
        }
        
        return value | (long)(head & 0x7F) << size * 7;
    }
}