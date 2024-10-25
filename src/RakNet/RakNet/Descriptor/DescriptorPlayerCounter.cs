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
/// Represents a player counter element for a <see cref="DescriptorBundle"/>.
/// This class keeps track of the number of players currently playing
/// and the maximum number of players allowed.
/// </summary>
/// <param name="playing">The current number of players currently playing.</param>
/// <param name="maxPlayers">The maximum number of players allowed.</param>
public class DescriptorPlayerCounter(int playing, int maxPlayers) : IDescriptorElement
{
    private int _playing = playing;
    private int _maxPlayers = maxPlayers;
    
    /// <summary>
    /// Gets or sets the number of players currently playing.
    /// The value is capped at the maximum number of players.
    /// </summary>
    public int Playing 
    { 
        get => _playing; 
        set => Interlocked.Exchange(ref _playing, Math.Min(value, _maxPlayers)); 
    }

    /// <summary>
    /// Gets or sets the maximum number of players allowed.
    /// The value cannot be set lower than the current number of players.
    /// </summary>
    public int MaxPlayers
    {
        get => _maxPlayers; 
        set => Interlocked.Exchange(ref _maxPlayers, Math.Max(value, _playing));
    }
    
    /// <summary>
    /// Returns a string representation of the player counter
    /// formatted as "<currentPlaying><separator><maxPlayers>".
    /// This representation will be included in the final description
    /// constructed by the <see cref="DescriptorBundle"/>.
    /// </summary>
    /// <returns>A string representing the current and maximum players.</returns>
    public string StringContent()
    {
        return $"{_playing}{DescriptorBundle.DescriptorSeparator}{_maxPlayers}";
    }
}