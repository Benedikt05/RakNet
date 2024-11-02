## Übersicht
Während der Offline-Sequenz erfolgt ein Ping-Pong-Austausch zwischen dem Client und dem Server. Die Pong-Antwort des Servers enthält ein Element namens ServerID, das anpassbaren Metadaten entspricht. Diese Metadaten bieten dem Client statistische Informationen, wie etwa Version, Protokoll, Mindest-Maximalspieler usw.

Die Klasse RakNetDescriptor ermöglicht die Verwaltung dieser Metadaten und wird später verwendet, um die Pong-Antwort zu erstellen.

## Einfaches Beispiel
```RakNetDescriptor``` funktioniert als ein Array, das die Elemente über numerische Indizes organisiert.


Die ```ToString()``` Methode gibt einen String mit allen Elementen zurück, die durch ";" getrennt sind, und bildet so die ServerID.
```csharp
var descriptor = new RakNetDescriptor();
descriptor.SetElement(0, "First");
descriptor.SetElement(2, "Third");
descriptor.SetElement(1, "Second");

// output: First;Second;Third;
Console.WriteLine(descriptor.ServerId);
```

## Verwendung des Operators
Wie im vorherigen Beispiel zu sehen, kann ```SetElement``` verwendet werden, um Elemente hinzuzufügen. RakNetDescriptor bietet jedoch auch die Möglichkeit, dasselbe mithilfe des +-Operators zu tun.
```csharp
var descriptor = new RakNetDescriptor();
descriptor += "First"; 
descriptor += "Second"; 
descriptor += "Third";

// output: First;Second;Third;
Console.WriteLine(descriptor.ServerId);
```

## Verwendung des Array-Indexers
Zusätzlich zu den zwei vorherigen Optionen, hat RakNetDescriptor die Fähigkeit, den Array-Indexer zu verwenden.

Initialisiere eine RakNetDescriptor Instanz mit initialen Daten.
```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hello",
    [1] = "World"
};

// output: Hello;World;
Console.WriteLine(descriptor.ServerId);
```
Aktualisieren oder Abrufen eines Elements an einem Index.
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
Console.WriteLine(descriptor.ServerId);
```
Beim Versuch, auf ein Element an einem Index zuzugreifen, das weder bei der Initialisierung der Klasse noch dynamisch hinzugefügt wurde, wird eine ```ArgumentNullException``` ausgelöst.

```csharp
var descriptor = new RakNetDescriptor
{
    [0] = "Hello",
    [1] = "World"
};

// output: ArgumentNullException
Console.WriteLine(descriptor[2]);
```
