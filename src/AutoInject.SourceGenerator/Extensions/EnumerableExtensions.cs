using System.Collections;
using System.Collections.Generic;

namespace RiseOn.AutoInject.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static object? GetValueOrDefault(this IDictionary<string, object?> dictionary, string  key)
            => dictionary.TryGetValue(key, out var value) ? value?.ToString() : null;
    }
}