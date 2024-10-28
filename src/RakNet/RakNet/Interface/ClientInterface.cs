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
using UdpClient = NetCoreServer.UdpClient;

namespace RakNet.Interface;

/// <summary>
/// Represents a client interface for UDP communication, initialized with a specific IP address and port.
/// Extends <see cref="NetCoreServer.UdpClient"/> to facilitate network interactions in a UDP-based client application.
/// </summary>
/// <param name="address">The IP address of the remote endpoint.</param>
/// <param name="port">The port number of the remote endpoint.</param>
internal sealed class ClientInterface(IPAddress address, int port) : UdpClient(address, port)
{
    protected override void OnConnected()
    {
        
    }

    protected override void OnDisconnected()
    {
        
    }

    protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
    {
        
    }

    protected override void OnError(SocketError error)
    {
        
    }
}