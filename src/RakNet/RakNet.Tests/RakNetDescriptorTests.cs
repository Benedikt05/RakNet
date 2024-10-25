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

namespace RakNet.Tests;

[TestFixture]
public class RakNetDescriptorTests
{
    [Test]
    [Description("Test adding elements to the descriptor using the '+=' operator")]
    public void AddElementUsingOperator()
    {
        var descriptor = new RakNetDescriptor();
        descriptor += "Hello";
        
        Assert.That(descriptor.ToString(), Is.EqualTo("Hello;"));
    }

    [Test]
    [Description("Test adding elements using SetElement to non-existing and specific indexes")]
    public void SetElementAtSpecificIndex()
    {
        var descriptor = new RakNetDescriptor();
        descriptor.SetElement(0, "First");
        descriptor.SetElement(2, "Third");
        descriptor.SetElement(1, "Second");
        
        Assert.That(descriptor.ToString(), Is.EqualTo("First;Second;Third;"));
    }

    [Test]
    [Description("Test updating element in an existing index")]
    public void SetElementAtExistingIndex()
    {
        var descriptor = new RakNetDescriptor();
        descriptor.SetElement(0, "OldValue");
        descriptor.SetElement(0, "NewValue");
        
        Assert.That(descriptor.ToString(), Is.EqualTo("NewValue;"));
    }

    [Test]
    [Description("Test that two descriptor instances are equal if contain the same values")]
    public void CompareObjectWithEqualValues()
    {
        var descriptor1 = new RakNetDescriptor();
        descriptor1.SetElement(0, "Value1");
        descriptor1.SetElement(1, "Value2");

        var descriptor2 = new RakNetDescriptor();
        descriptor2.SetElement(0, "Value1");
        descriptor2.SetElement(1, "Value2");
        
        Assert.That(descriptor1, Is.EqualTo(descriptor2));
    }

    [Test]
    [Description("Test that two descriptor instances are not equal if contain different values")]
    public void CompareObjectWithNotEqualValues()
    {
        var descriptor1 = new RakNetDescriptor();
        descriptor1.SetElement(0, "Value1");
        
        var descriptor2 = new RakNetDescriptor();
        descriptor2.SetElement(0, "DifferentValue");
        
        Assert.That(descriptor1, !Is.EqualTo(descriptor2));
    }

    [Test]
    [Description("Test that two descriptor instances generate the same hash if contain the same values")]
    public void ProducesSameHashCode()
    {
        var descriptor1 = new RakNetDescriptor();
        descriptor1.SetElement(0, "Value1");
        descriptor1.SetElement(1, "Value2");

        var descriptor2 = new RakNetDescriptor();
        descriptor2.SetElement(0, "Value1");
        descriptor2.SetElement(1, "Value2");
        
        Assert.That(descriptor1.GetHashCode(), Is.EqualTo(descriptor2.GetHashCode()));
    }

    [Test]
    [Description("Test that two descriptor instances generate different hash if contain different values")]
    public void ProducesDifferentHashCodes()
    {
        var descriptor1 = new RakNetDescriptor();
        descriptor1.SetElement(0, "Value1");

        var descriptor2 = new RakNetDescriptor();
        descriptor2.SetElement(0, "DifferentValue");
        
        Assert.That(descriptor1.GetHashCode(), !Is.EqualTo(descriptor2.GetHashCode()));
    }
}