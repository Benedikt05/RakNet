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

using System.Collections.Concurrent;

namespace RakNet;

/// <summary>
/// Manages periodic execution for multiple RakNet services.
/// This class enables parallel execution of tasks for registered services,
/// distributing workload efficiently across a configurable number of background tasks.
/// </summary>
public sealed class RakNetTicker : IDisposable
{
    private readonly List<Task> _tasks = [];
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly ConcurrentQueue<RakNetServiceBase> _serviceQueue = new();
    
    private readonly int _updateInterval;
    private bool _disposed;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RakNetTicker"/> class
    /// with a specified number of parallel tasks to handle service execution.
    /// </summary>
    /// <param name="parallelTasks">The number of parallel tasks for executing registered services.</param>
    /// <param name="updateInterval">Delay in ms between updates.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if parallelTasks is less than 1.</exception>
    /// /// <exception cref="ArgumentOutOfRangeException">Thrown if updateInterval is less than 20.</exception>
    public RakNetTicker(int parallelTasks = 1, int updateInterval = 20)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(parallelTasks, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(updateInterval, 20);
        
        _updateInterval = updateInterval;
        
        for (var i = 0; i < parallelTasks; i++)
        {
            _tasks.Add(Task.Factory.StartNew(() => TaskWorkerAsync(_cancellationTokenSource.Token), 
                _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default));
        }
    }
    
    /// <summary>
    /// Registers a new RakNet service to be ticked periodically.
    /// </summary>
    /// <param name="service">The RakNet service to register.</param>
    public void TickService(RakNetServiceBase service)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RakNetTicker));
        _serviceQueue.Enqueue(service);
    }
    
    private async Task TaskWorkerAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_serviceQueue.TryDequeue(out var service))
            {
                if (service.Running)
                {
                    await service.UpdateAsync();
                }
                
                _serviceQueue.Enqueue(service);
            }

            await Task.Delay(_updateInterval, cancellationToken);
        }
    }

    /// <summary>
    /// Disposes the RakNetTicker and stops all periodic tasks.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        
        _cancellationTokenSource.Cancel();
        Task.WhenAll(_tasks).Wait();
        _cancellationTokenSource.Dispose();
        _serviceQueue.Clear();
        _tasks.Clear();
        _disposed = true;
    }
}