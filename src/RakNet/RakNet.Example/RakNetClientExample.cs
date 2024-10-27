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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RakNet.Example;

/// <summary>
/// This is an example of how to use RakNetClient to connect to an existing Minecraft Bedrock server instance,
/// as well as to a RakNetServer instance.
/// </summary>
/// <param name="logger"></param>
public class RakNetClientExample(ILogger<RakNetClientExample> logger) : IHostedService
{
    public const string ServiceId = "client";
    
    private RakNetTicker? _ticker;
    private RakNetClient? _client;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ticker ??= new RakNetTicker(updateInterval: 50);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_client != null)
        {
            _client.StopService();
            _ticker?.StopTickService(_client);
            
            logger.Log(LogLevel.Information, "RakNetClientExample stopped");
        }
        
        _ticker?.Dispose();
        logger.Log(LogLevel.Information, "RakNetClientExample resources free after stopping");
        
        return Task.CompletedTask;
    }
}