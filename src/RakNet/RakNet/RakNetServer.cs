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
/// Represents a RakNet server for managing network communications. Initializes a new instance 
/// of the <see cref="RakNetServer"/> class with the specified IP address and port.
/// Inherits from <see cref="RakNetServiceBase"/>.
/// </summary>
/// <param name="address">The IP address where the server will listen for connections.</param>
/// <param name="port">The port number on which the server will accept connections.</param>
public sealed class RakNetServer(IPAddress address, int port) : RakNetServiceBase(address, port)
{
    /// <summary>
    /// Gets or sets the `RakNetDescriptor` instance, which provides detailed metadata descriptions 
    /// and configuration for offline pong.
    /// </summary>
    public RakNetDescriptor? Descriptor { get; set; }
    
    private ServerInterface? _interface;
    
    protected override void Start()
    {
        ArgumentNullException.ThrowIfNull(Descriptor);
        
        _interface ??= new ServerInterface(Address, Port);
        if (!_interface.Start()) throw new InvalidOperationException("Failed to start RakNetServer.");
    }

    protected override void Stop()
    {
        _interface?.Stop();
        _interface = null;
    }
    
    internal override Task UpdateAsync()
    {
        return Task.CompletedTask;
    }
}