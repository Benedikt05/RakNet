## Overview

RakNetTicker is a key component designed to manage periodic updates for multiple instances of RakNetServer or RakNetClient. 
In networked applications, especially in gaming or real-time systems, it's common to have multiple services that require 
regular updates or tasks to be executed concurrently. However, running a separate task for each instance can lead to performance 
issues, increased resource consumption, and complexity in managing those tasks.

## Functionality and Purpose
Provides a centralized mechanism for handling periodic updates across multiple services. Instead of each service running 
its own update loop, RakNetTicker takes responsibility for executing updates in a controlled and efficient manner. 
When services are registered with RakNetTicker, it distributes the execution of their periodic tasks across a defined number 
of independent worker tasks. This allows for better resource management and ensures that no single thread is overwhelmed 
with too many concurrent updates.

Implementing RakNetTicker simplifies the design of the networking components. Developers can focus on the logic of their
RakNetServer or RakNetClient implementations without having to worry about the intricacies of task scheduling and execution.

## Examples of Initiation
All services such as RakNetServer or RakNetClient need to register with a RakNetTicker to update connections and handle 
packets sequences.

An instance of RakNetTicker can be created with default parameters. This instance would use a single worker task and be updated every 20 ms.
```csharp
var ticker = new RakNetTicker();
```

To increase the number of worker task, 'parallelTasks' must be specified in the constructor, with a minimum value of 1.
```csharp
var ticker = new RakNetTicker(parallelTasks: 5);
```

To increase the update time, 'updateInterval' must be specified in the constructor, with a minimum value of 20ms.
```csharp
var ticker = new RakNetTicker(updateInterval: 50);
```

It is also possible to set both parameters in the constructor.
```csharp
var ticker = new RakNetTicker(2, 50); // workers task, time in ms
```

ArgumentOutOfRangeException exception will be thrown if the parameters passed in the constructor are below the minimum values:

- parallelTasks: 1
- updateInterval: 20

## Updates in Services
To instruct RakNetTicker to start performing updates on an service, it is sufficient to provide the service.
The service needs to be running before registering it or you might get an InvalidOperationException.
```csharp
ticker.StartTickService(raknetServer);
```

To instruct RakNetTicker to stop updates on an service, it is sufficient to provide the service.
The service needs to be stopped before unregistering it, or you might get an InvalidOperationException.
```csharp
ticker.StopTickService(raknetServer);
```

## Cleaning Resources
To terminate the execution of RakNetTicker, it is important to first remove all services from the ticker to avoid synchronization 
issues when closing the service connections. Once this is done, it is sufficient to call Dispose.
```csharp
ticker.Dispose();
```