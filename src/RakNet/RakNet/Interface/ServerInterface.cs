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
using System.Net.Sockets;
using NetCoreServer;
using Buffer = System.Buffer;

namespace RakNet.Interface;

/// <summary>
/// Represents a server interface for UDP communication, initialized with a specific IP address and port.
/// Extends <see cref="NetCoreServer.UdpServer"/> to manage UDP-based network interactions on the server side.
/// </summary>
/// <param name="address">The IP address on which the server will listen.</param>
/// <param name="port">The port number on which the server will listen.</param>
internal sealed class ServerInterface(IPAddress address, int port) : UdpServer(address, port)
{
    public event EventHandler<(EndPoint, byte[])>? ReceiveBufferEvent;
    public event EventHandler<SocketError>? ErrorEvent;
    
    protected override void OnStarted()
    {
        ReceiveAsync();
    }

    protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
    {
        if (size == 0)
        {
            // Continue listening for new messages async.
            ReceiveAsync();
            return;
        }
        
        // The buffer can sometimes be larger than it actually contains valid bytes.
        // Only process valid bytes using offset and size as a guide.
        var data = new byte[size];
        Buffer.BlockCopy(buffer, (int)offset, data, 0, (int)size);
        
        ReceiveBufferEvent?.Invoke(this, (endpoint, data));
        
        // Continue listening for new messages async.
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        ErrorEvent?.Invoke(this, error);
    }
}