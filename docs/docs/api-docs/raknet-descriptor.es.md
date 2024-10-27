## Descripción General

Durante la secuencia fuera de línea, se produce un intercambio de ping-pong entre el cliente y el servidor. La respuesta
de pong del servidor incluye un elemento llamado ServerID, que corresponde a metadatos personalizables. Estos metadatos
proporcionan información estadística al cliente, como la versión, el protocolo, el número mínimo y máximo de jugadores,
etc.

RakNetDescriptor es una clase que permite gestionar estos metadatos y que se utiliza posteriormente para construir la
respuesta de pong.

## Ejemplo Básico

```RakNetDescriptor``` funciona como un arreglo, utilizando índices numéricos para organizar los elementos.

El método ```ToString()``` devuelve una cadena con todos los elementos separados por ';', que es el ServerID..

```csharp
var descriptor = new RakNetDescriptor();
descriptor.SetElement(0, "Primero");
descriptor.SetElement(2, "Tercero");
descriptor.SetElement(1, "Segundo");

// salida: Primero;Segundo;Tercero;
Console.WriteLine(descriptor.ServerId);
```

## Usando Operador

Como se vio en el ejemplo anterior, ```SetElement``` se puede utilizar para agregar elementos; sin embargo,
RakNetDescriptor
también ofrece la opción de hacerlo utilizando el operador ```+```.

```csharp
var descriptor = new RakNetDescriptor();
descriptor += "Primero"; 
descriptor += "Segundo"; 
descriptor += "Tercero";

// salida: Primero;Segundo;Tercero;
Console.WriteLine(descriptor.ServerId);
```

## Usando Array indexado

Además de las dos opciones anteriores, RakNetDescriptor tiene la capacidad de usar el indexador de arreglo.

Inicializa una instancia de RakNetDescriptor con datos iniciales

```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hola",
    [1] = "Mundo"
};

// salida: Hola;Mundo;
Console.WriteLine(descriptor.ServerId);
```

Puedes actualizar o recuperar un elemento en un índice específico.

```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hola",
    [1] = "Mundo"
};

// Update data
descriptor[1] = "RakNet";

// salida: Hola
Console.WriteLine(descriptor[0]);

// salida: Hola;RakNet;
Console.WriteLine(descriptor.ServerId);
```

Al intentar acceder a un elemento en un índice que no se añadió durante la inicialización de la clase o dinámicamente,
se obtendrá una ```ArgumentNullException```.

```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hola",
    [1] = "Mundo"
};

// output: ArgumentNullException
Console.WriteLine(descriptor[2]);
```