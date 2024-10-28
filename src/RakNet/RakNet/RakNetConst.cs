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

using System.Net;

namespace RakNet;

/// <summary>
/// Provides constants, unique identifiers, and common utility functions used across the RakNet.
/// This class centralizes shared values and methods to maintain consistency and avoid redundancy.
/// </summary>
internal static class RakNetConst
{
    public const int ProtocolVersion = 11;
    
    public const int MagicSize = 16;
    public static readonly byte[] Magic = {
        0x00, 0xFF, 0xFF, 0x00,
        0xFE, 0xFE, 0xFE, 0xFE,
        0xFD, 0xFD, 0xFD, 0xFD,
        0x12, 0x34, 0x56, 0x78
    };
    
    public const short MtuMax = 1490;
    public const short MtuMin = 478;
    
    public const byte UnconnectedPingId = 0x01;
    public const byte UnconnectedPongId = 0x1c;

    public const byte OpenConnectionRequest1Id = 0x05;
    public const byte OpenConnectionRequest2Id = 0x07;
    public const byte OpenConnectionReply1Id = 0x06;
    public const byte OpenConnectionReply2Id = 0x08;

    public const byte IncompatibleProtocolId = 0x19;

    public const sbyte AckId = -64;
    public const sbyte NackId = -96;

    public const sbyte FrameSetId = -124;

    public const byte ConnectionRequestId = 0x09;
    public const byte ConnectionRequestAcceptedId = 0x10;
    public const byte NewIncomingConnectionId = 0x13;
    public const byte DisconnectId = 0x15;
    public const byte GamePacketId = 0xfe;

    public const byte ConnectedPingId = 0x00;
    public const byte ConnectedPongId = 0x03;

    public static readonly IPEndPoint DummyFirstAddress = new(IPAddress.Parse("127.0.0.1"), 0);
    public static readonly IPEndPoint DummyFillAddress = new(IPAddress.Parse("0.0.0.0"), 0);

    public static long GenerateTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    
    public static long GenerateGuid()
    {
        return BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
    }
}