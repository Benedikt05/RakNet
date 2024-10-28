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
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance.Buffers;

namespace RakNet.Binary.Writer;

/// <summary>
/// Provides static methods for writing specialized or complex data structures to a binary buffer.
/// This class handles custom serialization needs beyond primitive data types,
/// enabling efficient storage and retrieval of complex data formats.
/// </summary>
internal static class BinarySpecialWriter
{
    public static void WriteInt24(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, int value)
    {
        Span<byte> bytes = stackalloc byte[BinaryDataSize.Int24BinarySize];
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                bytes[0] = (byte)(value & 0xFF);
                bytes[1] = (byte)((value >> 8) & 0xFF);
                bytes[2] = (byte)((value >> 16) & 0xFF);
                break;
            case BinaryEncoding.BigEndian:
                bytes[0] = (byte)((value >> 16) & 0xFF);
                bytes[1] = (byte)((value >> 8) & 0xFF);
                bytes[2] = (byte)(value & 0xFF);
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding}");
        }
        
        buffer.Write(bytes);
    }

    public static void WriteUInt24(BinaryEncoding encoding, MemoryBufferWriter<byte> buffer, uint value)
    {
        Span<byte> bytes = stackalloc byte[BinaryDataSize.Int24BinarySize];
        switch (encoding)
        {
            case BinaryEncoding.LittleEndian:
                bytes[0] = (byte)(value & 0xFF);
                bytes[1] = (byte)((value >> 8) & 0xFF);
                bytes[2] = (byte)((value >> 16) & 0xFF);
                break;
            case BinaryEncoding.BigEndian:
                bytes[0] = (byte)((value >> 16) & 0xFF);
                bytes[1] = (byte)((value >> 8) & 0xFF);
                bytes[2] = (byte)(value & 0xFF);
                break;
            default:
                throw new InvalidDataException($"Unexpected BinaryEncoding value {encoding}");
        }
        
        buffer.Write(bytes);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteVarInt32(MemoryBufferWriter<byte> buffer, int value)
    {
        WriteUnsignedVarInt32(buffer, (value << 1) ^ (value >> 31));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUnsignedVarInt32(MemoryBufferWriter<byte> buffer, int value)
    {
        int temp;
        
        do {
            temp = value;
            value >>>= 7;
            BinaryPrimitiveWriter.WriteByte(buffer, value != 0 ? (byte) (temp | 0x80) : (byte) temp);
        } while (value != 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteVarInt64(MemoryBufferWriter<byte> buffer, long value)
    {
        WriteUnsignedVarInt64(buffer, (value << 1) ^ (value >> 63));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUnsignedVarInt64(MemoryBufferWriter<byte> buffer, long value)
    {
        long temp;
        
        do {
            temp = value;
            value >>>= 7;
            BinaryPrimitiveWriter.WriteByte(buffer, value != 0L ? (byte) (temp | 0x80L) : (byte) temp);
        } while (value != 0L);
    }
}