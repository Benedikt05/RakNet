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
using RakNet.Interface;

namespace RakNet;

/// <summary>
/// Represents a RakNet client for connecting to a RakNet server. Initializes a new instance 
/// of the <see cref="RakNetClient"/> class with the specified IP address and port.
/// Inherits from <see cref="RakNetServiceBase"/>.
/// </summary>
/// <param name="address">The IP address of the server to which the client will connect.</param>
/// <param name="port">The port number on which the server is listening for connections.</param>
public sealed class RakNetClient(IPAddress address, int port) : RakNetServiceBase(address, port)
{
    private ClientInterface? _interface;
    
    protected override void Start()
    {
        _interface ??= new ClientInterface(Address, Port);
        if (!_interface.Connect()) throw new InvalidOperationException("Failed to start RakNetClient.");
    }

    protected override void Stop()
    {
        _interface?.Disconnect();
        _interface = null;
    }

    internal override Task UpdateAsync()
    {
        return Task.CompletedTask;
    }
}