using System;
using System.Diagnostics.CodeAnalysis;

namespace Hw7Tests.Shared;

[ExcludeFromCodeCoverage]
public static class StringExtensions
{
    public static string RemoveNewLine(this string src)
    {
        if (src is null) throw new ArgumentNullException(nameof(src));
        return src.Replace("\r", "").Replace("\n", "");
    }
}