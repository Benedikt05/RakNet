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

namespace RakNet.Binary.Reader;

/// <summary>
/// Provides static methods for reading specialized binary data formats.
/// Designed for reading specific data types or custom binary structures efficiently.
/// </summary>
internal static class BinarySpecialReader
{
    public static int ReadInt24(BinaryEncoding encoding, ref int position, ReadOnlySpan<byte> buffer)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryDataSize.Int24BinarySize + 1);
        
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
        ArgumentOutOfRangeException.ThrowIfLessThan(position, buffer.Length - BinaryDataSize.Int24BinarySize + 1);
        
        return encoding == BinaryEncoding.LittleEndian
            ? (uint)(buffer[position++] | (buffer[position++] << 8) | (buffer[position++] << 16))
            : (uint)((buffer[0] << position++) | (buffer[position++] << 8) | buffer[position++]);
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
            head = BinaryPrimitiveReader.ReadByte(ref position, buffer);
            result |= (head & 0x7F) << (7 * size++);

            if ((head & 0x80) != 0x80 || size >= 6) {
                break;
            }
        }

        return result;
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
        
        while (((head = BinaryPrimitiveReader.ReadByte(ref position, buffer)) & 0x80) != 0) {
            value |= (long) (head & 0x7F) << size++ * 7;
            if (size > 10) throw new OverflowException("VarLong too big");
        }
        
        return value | (long)(head & 0x7F) << size * 7;
    }
}