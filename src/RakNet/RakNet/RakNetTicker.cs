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
    private readonly HashSet<RakNetServiceBase> _tickServices = [];
    
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
    /// Registers a service to receive periodic updates.
    /// This method adds the specified service to the update queue,
    /// allowing it to be processed in the periodic task execution.
    /// </summary>
    /// <param name="service">The service to be registered.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the ticker has been disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when attempting to register a service that is not running.</exception>
    public void StartTickService(RakNetServiceBase service)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RakNetTicker));
        
        if (!service.Running) throw new InvalidOperationException("The service is not running.");
        
        _serviceQueue.Enqueue(service);
        _tickServices.Add(service);
    }

    /// <summary>
    /// Unregisters a service from receiving periodic updates.
    /// This method removes the specified service from the registered services
    /// and ensures it no longer appears in the update queue.
    /// </summary>
    /// <param name="service">The service to be unregistered.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the ticker has been disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when attempting to register a service that is running.</exception>
    public void StopTickService(RakNetServiceBase service)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RakNetTicker));
        
        if (service.Running) throw new InvalidOperationException("The service is running.");
        
        _tickServices.Remove(service);
        
        var remainingServices = _serviceQueue.Where(queueService => _tickServices.Contains(queueService)).ToList();
        
        _serviceQueue.Clear();
        foreach (var filterService in remainingServices)
        {
            _serviceQueue.Enqueue(filterService);
        }
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
        _tickServices.Clear();
        _tasks.Clear();
        _disposed = true;
    }
}