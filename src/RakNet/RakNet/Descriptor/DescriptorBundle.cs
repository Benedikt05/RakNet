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

namespace RakNet.Descriptor;

/// <summary>
/// Represents a bundle of descriptor elements that can be combined
/// to form a comprehensive description.
/// The combined content is used as serverId in OfflinePong to display a status.
/// </summary>
public class DescriptorBundle
{
    internal const char DescriptorSeparator = ';';
    internal string Content { get; private set; } = string.Empty;
    
    private readonly List<IDescriptorElement> _elements = [];
    
    /// <summary>
    /// Adds an element to the descriptor bundle.
    /// </summary>
    /// <param name="element">The element to add.</param>
    /// <returns>The current instance for chaining.</returns>
    public DescriptorBundle Append(IDescriptorElement element)
    {
        ArgumentNullException.ThrowIfNull(element);
        _elements.Add(element);
        
        Update();
        
        return this;
    }
    
    /// <summary>
    /// Adds a collection of elements to the descriptor bundle.
    /// </summary>
    /// <param name="elements">The collection of elements to add.</param>
    /// <returns>The current instance for chaining.</returns>
    public DescriptorBundle Append(IEnumerable<IDescriptorElement> elements)
    {
        ArgumentNullException.ThrowIfNull(elements);
        
        foreach (var element in elements)
        {
            Append(element);
        }
        
        return this;
    }

    internal void Update()
    {
        Content = string.Join(DescriptorSeparator, _elements.Select(element => element.StringContent()));
    }

    /// <summary>
    /// Returns the combined content string representing all descriptor elements.
    /// </summary>
    /// <returns>A string containing the combined content of all descriptor elements.</returns>
    public override string ToString()
    {
        return Content;
    }
}