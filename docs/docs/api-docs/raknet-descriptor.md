During the offline sequence, a ping-pong exchange occurs between the client and the server. The pong response from the server 
contains an element called ServerID, which corresponds to customizable metadata. This metadata provides statistical information 
to the client, such as version, protocol, minimum-maximum players, etc.

RakNetDescriptor is a class that enables managing this metadata and is later used to construct the pong response.

## Basic Example
RakNetDescriptor functions as an array using numeric index to organize the elements. 

The ToString() method returns a string with all elements separated by ';' which is the ServerID
```csharp
var descriptor = new RakNetDescriptor();
descriptor.SetElement(0, "First");
descriptor.SetElement(2, "Third");
descriptor.SetElement(1, "Second");

// output: First;Second;Third;
Console.WriteLine(descriptor.ToString());
```

## Using Operator
As seen in the previous example, setElement can be used to add elements; however, RakNetDescriptor also offers the option 
to do the same using the '+' operator.
```csharp
var descriptor = new RakNetDescriptor();
descriptor += "First"; 
descriptor += "Second"; 
descriptor += "Third";

// output: First;Second;Third;
Console.WriteLine(descriptor.ToString());
```

## Using Array Indexer
In addition to the two previous options, RakNetDescriptor has the ability to use the array indexer.

Initialize an instance of RakNetDescriptor with initial data.
```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hello",
    [1] = "World"
};

// output: Hello;World;
Console.WriteLine(descriptor.ToString());
```

Update or retrieve an element at an index.
```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hello",
    [1] = "World"
};

// Update data
descriptor[1] = "RakNet";

// output: Hello
Console.WriteLine(descriptor[0]);

// output: Hello;RakNet;
Console.WriteLine(descriptor.ToString());
```

When attempting to access an element at an index that was not added during the class initialization or dynamically, 
an ArgumentNullException will be obtained.
```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hello",
    [1] = "World"
};

// output: ArgumentNullException
Console.WriteLine(descriptor[2]);
```