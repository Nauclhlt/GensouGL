using System.Text.RegularExpressions;

namespace GensouGL.Graphics;

internal static partial class ShaderPreprocessor
{
    private static Assembly _asm;

    public static void SetSourceAssembly(Assembly asm)
    {
        _asm = asm;
    }

    private static string ReadLibrary(string libraryName)
    {
        using Stream stream = _asm.GetManifestResourceStream($"GensouGL.Graphics.Shaders.Library.{libraryName}.glsl");
        if (stream is null) return null;

        using StreamReader reader = new(stream, true);

        return reader.ReadToEnd();
    }

    public static string ExpandInclude(string shaderSource)
    {
        ReadOnlySpan<char> span = shaderSource;
        const string IncludeHeader = "//include<";
        const string IgnoreSectionOpen = "//ignore";
        const string IgnoreSectionEnd = "//ignore_end";

        StringBuilder sb = new(shaderSource.Length * 2);

        for (int i = 0; i < span.Length; i++)
        {
            ReadOnlySpan<char> s = span.Slice(i);
            if (s.StartsWith(IncludeHeader))
            {
                int closing = i + span.Slice(i).IndexOf('>');
                int nameLength = closing - i - IncludeHeader.Length;
                string targetName = shaderSource.Substring(i + IncludeHeader.Length, nameLength);

                string libraryBody = ReadLibrary(targetName);
                if (libraryBody is null)
                {
                    DebugLog.Error($"{nameof(ShaderPreprocessor)}: Library not found.");
                    continue;
                }
                string expandedLibraryBody = ExpandInclude(libraryBody);

                sb.Append(expandedLibraryBody);
                i = closing;
            }
            else if (s.StartsWith(IgnoreSectionOpen))
            {
                while (!span.Slice(i).StartsWith(IgnoreSectionEnd))
                {
                    i++;
                }
                i += IgnoreSectionEnd.Length - 1;
            }
            else
            {
                sb.Append(span[i]);
            }
        }

        return sb.ToString();
    }
}