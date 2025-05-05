using System.Collections;
using System.Collections.Generic;

namespace RiseOn.AutoInject.Extensions
{
    public static class EnumerableExtensions
    {
        public static object? GetValueOrDefault(this IDictionary<string, object?> dictionary, string  key)
            => dictionary.TryGetValue(key, out var value) ? value?.ToString() : null;
    }
}