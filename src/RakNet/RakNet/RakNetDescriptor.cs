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

namespace RakNet;

/// <summary>
/// Represents a descriptor for RakNet with index-value elements, supporting
/// equality checks, element addition via '+' operator, and string conversion.
/// The combined content is used as serverId in OfflinePong to display a status.
/// </summary>
public class RakNetDescriptor : IEqualityComparer<RakNetDescriptor>
{
    private readonly Dictionary<int, string> _elements = new();

    /// <summary>
    /// Gets or sets the element at the specified index within the descriptor.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The value at the specified index as a <see cref="string"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the index does not exist or the value is null.</exception>
    public string this[int index]
    {
        get
        {
            if (_elements.TryGetValue(index, out var value))
            {
                return value;
            }
            throw new ArgumentNullException(nameof(index));
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _elements[index] = value;
        }
    }
    
    /// <summary>
    /// Adds a new string element to the descriptor using the + operator.
    /// </summary>
    /// <param name="descriptor">The RakNetDescriptor instance.</param>
    /// <param name="value">The string value to add.</param>
    /// <returns>A new RakNetDescriptor instance with the added element.</returns>
    public static RakNetDescriptor operator +(RakNetDescriptor descriptor, string value)
    {
        descriptor.SetElement(descriptor._elements.Count, value);
        return descriptor;
    }
    
    /// <summary>
    /// Checks equality between two RakNetDescriptor instances.
    /// </summary>
    /// <param name="left">The first RakNetDescriptor instance.</param>
    /// <param name="right">The second RakNetDescriptor instance.</param>
    /// <returns>True if both descriptors are equal; otherwise, false.</returns>
    public static bool operator ==(RakNetDescriptor left, RakNetDescriptor right)
    {
        return ReferenceEquals(left, right) || left.Equals(right);
    }

    /// <summary>
    /// Checks inequality between two RakNetDescriptor instances.
    /// </summary>
    /// <param name="left">The first RakNetDescriptor instance.</param>
    /// <param name="right">The second RakNetDescriptor instance.</param>
    /// <returns>True if both descriptors are not equal; otherwise, false.</returns>
    public static bool operator !=(RakNetDescriptor left, RakNetDescriptor right)
    {
        return !(left == right);
    }
    
    /// <summary>
    /// Set or updates an element at the specified index.
    /// </summary>
    /// <param name="index">The index at which to set or update the element.</param>
    /// <param name="value">The value to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
    public void SetElement(int index, string value)
    {
        _elements[index] = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Returns the descriptor as a single concatenated string with elements separated by ";".
    /// </summary>
    /// <returns>A string representation of the descriptor.</returns>
    public override string ToString()
    {
        return $"{string.Join(";", _elements.OrderBy(pair => pair.Key).Select(pair => pair.Value))};";
    }
    
    /// <summary>
    /// Checks equality between the current instance and another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>True if the other object is equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        
        return obj is RakNetDescriptor other && Equals(this, other);
    }

    /// <summary>
    /// Checks equality between two RakNetDescriptor instances based on their elements.
    /// </summary>
    /// <param name="x">The first RakNetDescriptor instance.</param>
    /// <param name="y">The second RakNetDescriptor instance.</param>
    /// <returns>True if both descriptors have the same elements; otherwise, false.</returns>
    public bool Equals(RakNetDescriptor? x, RakNetDescriptor? y)
    {
        if (x is null && y is null) return true;
        
        if (x is null || y is null) return false;
        
        if (x._elements.Count != y._elements.Count) return false;

        foreach (var pair in x._elements)
        {
            if (!y._elements.TryGetValue(pair.Key, out var otherValue) || pair.Value != otherValue)
            {
                return false;
            }
        }

        return true;
    }
    
    /// <summary>
    /// Computes a hash code for the current descriptor.
    /// </summary>
    /// <returns>The hash code for the descriptor.</returns>
    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    /// <summary>
    /// Computes a hash code for a given RakNetDescriptor instance based on its elements.
    /// </summary>
    /// <param name="obj">The RakNetDescriptor instance to compute the hash code for.</param>
    /// <returns>The computed hash code.</returns>
    public int GetHashCode(RakNetDescriptor obj)
    {
        var hash = 17;
        foreach (var pair in obj._elements)
        {
            hash = hash * 31 + pair.Key.GetHashCode();
            hash = hash * 31 + pair.Value.GetHashCode();
        }
        
        return hash;
    }
}