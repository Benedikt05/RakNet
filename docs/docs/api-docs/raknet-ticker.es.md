## Descripción General
RakNetTicker es un componente clave diseñado para gestionar actualizaciones periódicas para múltiples instancias de 
RakNetServer o RakNetClient. En aplicaciones de red, especialmente en juegos o sistemas en tiempo real, es común tener varios
servicios que requieren actualizaciones regulares o tareas que se ejecuten de manera concurrente. Sin embargo, ejecutar una 
tarea separada para cada instancia puede provocar problemas de rendimiento, un aumento en el consumo de recursos y complicaciones 
en la gestión de estas tareas.


## Funcionalidad y Propósito
RakNetTicker proporciona un mecanismo centralizado para manejar actualizaciones periódicas a través de múltiples servicios. 
En lugar de que cada servicio ejecute su propio bucle de actualización, RakNetTicker se encarga de ejecutar las actualizaciones 
de manera controlada y eficiente. Cuando los servicios se registran en RakNetTicker, este distribuye la ejecución de sus 
tareas periódicas entre un número definido de tareas trabajadoras independientes. Esto permite una mejor gestión de recursos 
y asegura que ningún hilo se vea abrumado por demasiadas actualizaciones concurrentes.

La implementación de RakNetTicker simplifica el diseño de los componentes de red. Los desarrolladores pueden concentrarse
 en la lógica de sus implementaciones de RakNetServer o RakNetClient sin tener que preocuparse por las complejidades de la
programación y ejecución de tareas.

## Examples of Initiation
Todos los servicios como ```RakNetServer``` o ```RakNetClient``` deben registrarse con un RakNetTicker para actualizar las conexiones
y manejar secuencias de paquetes.

Se puede crear una instancia de ```RakNetTicker``` con parámetros predeterminados. Esta instancia utilizaría una sola tarea de
trabajo y se actualizaría cada 20 ms.
```csharp
var ticker = new RakNetTicker();
```

Para aumentar el número de tareas trabajadoras, se debe especificar 'parallelTasks' en el constructor, con un valor mínimo de 1.
```csharp
var ticker = new RakNetTicker(parallelTasks: 5);
```

Para aumentar el intervalo de actualización, se debe especificar 'updateInterval' en el constructor, con un valor mínimo de 20 ms.
```csharp
var ticker = new RakNetTicker(updateInterval: 50);
```

También es posible establecer ambos parámetros en el constructor.
```csharp
var ticker = new RakNetTicker(2, 50); // tareas trabajadoras, tiempo en ms
```

Se lanzará una excepción ```ArgumentOutOfRangeException``` si los parámetros pasados en el constructor están por debajo de los valores mínimos:

- parallelTasks: 1
- updateInterval: 20

## Actualizaciones en los Servicios
Para indicar a RakNetTicker que comience a realizar actualizaciones en un servicio, es suficiente con proporcionar el servicio.
Este debe estar en funcionamiento antes de registrarlo, o podrías recibir una ```InvalidOperationException```.
```csharp
ticker.StartTickService(raknetServer);
```

Para indicar a RakNetTicker que detenga las actualizaciones en un servicio, simplemente proporciona el servicio. Este debe 
estar detenido antes de anular su registro, o podrías recibir una ```InvalidOperationException```.
```csharp
ticker.StopTickService(raknetServer);
```

## Limpieza de Recursos
Para finalizar la ejecución de RakNetTicker, es importante eliminar primero todos los servicios del ticker para evitar 
problemas de sincronización al cerrar las conexiones de servicio. Una vez hecho esto, es suficiente con llamar a ```Dispose```.
```csharp
ticker.Dispose();
```