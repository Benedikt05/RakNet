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
/// Provides a base implementation for RakNet service operations.
/// This class defines common methods for starting and stopping a RakNet-based service.
/// It handles the management of the service lifecycle, including starting and stopping operations,
/// and provides a mechanism for executing periodic tasks asynchronously.
/// </summary>
/// <param name="address">The IP address to which the service will bind.</param>
/// <param name="port">The port number on which the service will listen.</param>
public abstract class RakNetServiceBase(IPAddress address, int port) : IRakNetService
{
    /// <summary>
    /// Specifies the IP address for the RakNet service to bind to.
    /// This property is initialized at object construction and is immutable thereafter.
    /// </summary>
    public readonly IPAddress Address = address;
    /// <summary>
    /// Specifies the port on which the RakNet service will listen.
    /// This property is initialized at object construction and is immutable thereafter.
    /// </summary>
    public readonly int Port = port;
    /// <summary>
    /// A unique identifier for the instance.Used to uniquely identify each instance within the RakNet.
    /// </summary>
    public readonly long Guid = RakNetConst.GenerateGuid();
    
    /// <summary>
    /// Tracks whether the service is currently running.
    /// </summary>
    public bool Running { get; private set; }
        
    /// <summary>
    /// Initiates the service start sequence.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when StartService is invoked and the service is already running.</exception>
    /// <exception cref="InvalidOperationException">Thrown when failed to start service.</exception>
    /// <exception cref="ArgumentNullException">Thrown when any argument requested by a service is null.</exception>
    public void StartService()
    {
        if (Running) throw new InvalidOperationException("The service is already running.");
        
        Start();
        Running = true;
    }

    /// <summary>
    /// Initiates the service stop sequence.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when StopService is invoked and the service is already stopped.</exception>
    public void StopService()
    {
        if (!Running) throw new InvalidOperationException("The service is already stopped.");
        
        Stop();
        Running = false;
    }
    
    protected abstract void Start();
    protected abstract void Stop();
    
    internal virtual Task UpdateAsync()
    {
        return Task.CompletedTask;
    }
}