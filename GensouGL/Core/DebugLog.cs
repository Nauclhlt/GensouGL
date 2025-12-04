namespace GensouGL.Core;

public static class DebugLog
{
    private static readonly StringBuilder _buffer;
    private static readonly StringWriter _writer;

    public static bool WriteToConsole { get; set; } = false;

    static DebugLog()
    {
        _buffer = new();
        _writer = new(_buffer);
    }

    internal static void Info(string message)
    {
        if (WriteToConsole) Console.WriteLine($"[INFO] {message}");
        _writer.Write($"[INFO] {message}");
    }

    internal static void Warn(string message)
    {
        if (WriteToConsole) Console.WriteLine($"[WARN] {message}");
        _writer.Write($"[WARN] {message}");
    }

    internal static void Error(string message)
    {
        if (WriteToConsole) Console.WriteLine($"[ERROR] {message}");
        _writer.Write($"[ERROR] {message}");
    }

    public static void User(string message)
    {
        if (WriteToConsole) Console.WriteLine($"[USER] {message}");
        _writer.Write($"[USER] {message}");
    }

    public static void Clear()
    {
        _buffer.Clear();
    }

    public static void SaveToFile(string filename)
    {
        File.WriteAllText(filename, _buffer.ToString());
    }
}