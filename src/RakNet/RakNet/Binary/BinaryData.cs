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

namespace RakNet.Binary;

/// <summary>
/// Specifies the encoding format for binary data representation in memory.
/// Determines the byte order (endianness) of multibyte values when read or written.
/// </summary
internal enum BinaryEncoding
{
    LittleEndian,
    BigEndian,
}

/// <summary>
/// Provides constants representing the binary size (in bytes) of various integer types.
/// Useful for determining fixed byte lengths in binary data processing.
/// </summary>
internal static class BinaryDataSize
{
    public const int Int8BinarySize = 1;
    public const int Int16BinarySize = 2;
    public const int Int24BinarySize = 3;
    public const int Int32BinarySize = 4;
    public const int Int64BinarySize = 8;
}