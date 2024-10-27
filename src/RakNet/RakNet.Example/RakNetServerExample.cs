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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RakNet.Example;

/// <summary>
/// This is an example of how to use RakNetServer to start a server for the popular Minecraft cube game in the
/// Bedrock edition that uses the RakNet protocol.
/// </summary>
public class RakNetServerExample(ILogger<RakNetServerExample> logger) : IHostedService
{
    public const string ServiceId = "server";
    
    private RakNetTicker? _ticker;
    private RakNetDescriptor? _descriptor;
    private RakNetServer? _server;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ticker ??= new RakNetTicker(updateInterval: 50);
        _descriptor ??= new RakNetDescriptor
        {
            [0] = "MCPE", // Edition
            [1] = "RakNet Server Example", // Motd Title
            [2] = "390", // Game Protocol
            [3] = "1.14.60", // Game Version
            [4] = "0", // Current Playing
            [5] = "100", // Max Players
            [6] = "13253860892328930865", // Server Guid
            [7] = "Test RakNet Lib", // Motd Subtitle
            [8] = "Survival", // Game mode
            [9] = "1", // Game mode id
            [10] = "19132" // Server port
        };

        _server ??= new RakNetServer(IPAddress.Any, 19132);
        _server.StartService();
        
        _ticker.StartTickService(_server);
        
        logger.Log(LogLevel.Information, "RaknetServerExample started");
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_server != null)
        {
            _server.StopService();
            _ticker?.StopTickService(_server);
            
            logger.Log(LogLevel.Information, "RaknetServerExample stopped");
        }
        
        _ticker?.Dispose();
        logger.Log(LogLevel.Information, "RaknetServerExample resources free after stopping");
        
        return Task.CompletedTask;
    }
}